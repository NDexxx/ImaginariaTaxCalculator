using System;
using System.IO;
using FluentValidation;
using FluentValidation.TestHelper;
using ImaginariaTaxCalculator.Helpers;
using ImaginariaTaxCalculator.Interfaces.Validators;
using ImaginariaTaxCalculator.Models;
using ImaginariaTaxCalculator.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace ImaginariaTaxCalculator.Tests
{
    public class ValidatorTests
    {
        private IValidator<TaxPayer> _payerValidator;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddScoped<IValidationHelper<RegexValidationHelper>, RegexValidationHelper>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

            services.AddOptions<Settings>().Bind(configuration.GetSection("Settings"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _payerValidator = new TaxPayerValidator(serviceProvider.GetService<IOptions<Settings>>(), serviceProvider.GetService<IValidationHelper<RegexValidationHelper>>());
        }

        [Test]
        public void Should_Have_Error_With_Wrong_Name_Input()
        {
            var input = new TaxPayer() { FullName = "Ivan", CharitySpent = 0, DateOfBirth = DateTime.UtcNow.AddYears(-15), GrossIncome = 1000, SSN = "12345" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.FullName);
            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Wrong_Charity_Input()
        {
            var input = new TaxPayer() { FullName = "Ivan Ivanov", CharitySpent = -1, DateOfBirth = DateTime.UtcNow.AddYears(-15), GrossIncome = 1000, SSN = "12345" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.CharitySpent);
            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Wrong_Birth_Date_Input()
        {
            var input = new TaxPayer() { FullName = "Ivan Ivanov", CharitySpent = 0, DateOfBirth = DateTime.UtcNow.AddYears(-11), GrossIncome = -1000, SSN = "12345" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.DateOfBirth);
            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Wrong_Gross_Income_Input()
        {
            var input = new TaxPayer() { FullName = "Ivan Ivanov", CharitySpent = 0, DateOfBirth = DateTime.UtcNow.AddYears(-15), GrossIncome = -1000, SSN = "12345" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.GrossIncome);
            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Wrong_SSN_Input()
        {
            var input = new TaxPayer() { FullName = "Ivan Ivanov", CharitySpent = 0, DateOfBirth = DateTime.UtcNow, GrossIncome = 1000, SSN = "2345" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.SSN);
            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Empty_Model()
        {
            var input = new TaxPayer();
            var result = _payerValidator.TestValidate(input);

            result.ShouldNotHaveValidationErrorFor(v => v.CharitySpent);

            result.ShouldHaveValidationErrorFor(v => v.FullName);
            result.ShouldHaveValidationErrorFor(v => v.DateOfBirth);
            result.ShouldHaveValidationErrorFor(v => v.GrossIncome);
            result.ShouldHaveValidationErrorFor(v => v.SSN);

            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Have_Error_With_Everyting_Wrong()
        {
            var input = new TaxPayer() { FullName = "Ivan", CharitySpent = -19, DateOfBirth = DateTime.UtcNow, GrossIncome = -1000, SSN = "123" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldHaveValidationErrorFor(v => v.FullName);
            result.ShouldHaveValidationErrorFor(v => v.CharitySpent);
            result.ShouldHaveValidationErrorFor(v => v.DateOfBirth);
            result.ShouldHaveValidationErrorFor(v => v.GrossIncome);
            result.ShouldHaveValidationErrorFor(v => v.SSN);

            Assert.False(result.IsValid);
        }

        [Test]
        public void Should_Not_Have_Error_With_Correct_Data()
        {
            var input = new TaxPayer() { FullName = "Ivan Ivanov", CharitySpent = 520, DateOfBirth = DateTime.UtcNow.AddYears(-20), GrossIncome = 3600, SSN = "123456" };
            var result = _payerValidator.TestValidate(input);

            result.ShouldNotHaveValidationErrorFor(v => v.FullName);
            result.ShouldNotHaveValidationErrorFor(v => v.CharitySpent);
            result.ShouldNotHaveValidationErrorFor(v => v.DateOfBirth);
            result.ShouldNotHaveValidationErrorFor(v => v.GrossIncome);
            result.ShouldNotHaveValidationErrorFor(v => v.SSN);

            Assert.True(result.IsValid);
        }
    }
}