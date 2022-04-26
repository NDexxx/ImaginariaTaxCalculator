using System.IO;
using ImaginariaTaxCalculator.Helpers.AmountFormatters;
using ImaginariaTaxCalculator.Helpers.Calculators;
using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Interfaces.Calculators;
using ImaginariaTaxCalculator.Models;
using ImaginariaTaxCalculator.Services.Calculators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace ImaginariaTaxCalculator.Tests
{
    public class TaxServiceTests
    {
        private ITaxService _taxService;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddScoped<ITaxCalculator<TaxCalculator>, TaxCalculator>();

            services.AddScoped<IAmountFormatter<CharityAmountFormatter>, CharityAmountFormatter>();
            services.AddScoped<IAmountFormatter<GrossAmountFormatter>, GrossAmountFormatter>();
            services.AddScoped<IAmountFormatter<SocialTaxAmountFormatter>, SocialTaxAmountFormatter>();
            services.AddScoped<ITaxCalculator<TaxCalculator>, TaxCalculator>();

            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<INetIncomeCalculator, NetIncomeCalculator>();
            services.AddScoped<ITotalTaxCalculator, TotalTaxCalculator>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

            services.AddOptions<Settings>().Bind(configuration.GetSection("Settings"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var options = serviceProvider.GetService<IOptions<Settings>>();

            var charityFormatter = serviceProvider.GetService<IAmountFormatter<CharityAmountFormatter>>();
            var grossAmountFormatter = serviceProvider.GetService<IAmountFormatter<GrossAmountFormatter>>();
            var socialTaxFormatter = serviceProvider.GetService<IAmountFormatter<SocialTaxAmountFormatter>>();
            var taxCalculator = serviceProvider.GetService<ITaxCalculator<TaxCalculator>>();
            var totalTaxCalculator = serviceProvider.GetService<ITotalTaxCalculator>();
            var netIncomeCalculator = serviceProvider.GetService<INetIncomeCalculator>();

            _taxService = new TaxService(options, charityFormatter, grossAmountFormatter, socialTaxFormatter, taxCalculator, totalTaxCalculator, netIncomeCalculator);
        }

        [TestCase(980, null)]
        public void Tax_Service_With_Wage_Under_Minimal_Tax_Amount(decimal grossIncome, decimal? charitySpent)
        {
            Taxes expected = new() { GrossIncome = grossIncome, NetIncome = grossIncome, CharitySpent = 0 };
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());
            AssertByPropertiesAndPass(expected, actual);
        }

        [TestCase(3400, null)]
        public void Tax_Service_With_Wage_Above_Max_Social_Contribution(decimal grossIncome, decimal? charitySpent)
        {
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());
            Taxes expected = new() { GrossIncome = grossIncome, IncomeTax = 240.00m, SocialTax = 300.00m, TotalTax = 540.00m, NetIncome = 2860.00m, CharitySpent = 0m };
            AssertByPropertiesAndPass(expected, actual);
        }

        [TestCase(2500, 150)]
        public void Tax_Service_With_Wage_And_Charity_In_Limits(decimal grossIncome, decimal? charitySpent)
        {
            Taxes expected = new() { GrossIncome = grossIncome, IncomeTax = 135, SocialTax = 202.5m, TotalTax = 337.5m, NetIncome = 2162.5m, CharitySpent = charitySpent };
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());

            AssertByPropertiesAndPass(expected, actual);
        }

        [TestCase(3600, 520)]
        public void Tax_Service_With_Wage_And_Charity_Above_Max(decimal grossIncome, decimal? charitySpent)
        {
            Taxes expected = new Taxes() { GrossIncome = grossIncome, CharitySpent = charitySpent, IncomeTax = 224, SocialTax = 300, TotalTax = 524, NetIncome = 3076 };
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());
            AssertByPropertiesAndPass(expected, actual);
        }

        [TestCase(1001, 1)]
        public void Tax_Service_With_Wage_Above_Min_Charity_Above_Max(decimal grossIncome, decimal? charitySpent)
        {
            Taxes expected = new Taxes() { GrossIncome = grossIncome, CharitySpent = charitySpent, IncomeTax = 0, SocialTax = 0, TotalTax = 0, NetIncome = grossIncome };
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());

            AssertByPropertiesAndPass(expected, actual);
        }

        [TestCase(1001, 2000)]
        public void Tax_Service_With_Charty_Larger_Than_Gross(decimal grossIncome, decimal? charitySpent)
        {
            Taxes expected = new Taxes() { GrossIncome = grossIncome, CharitySpent = charitySpent, IncomeTax = 0, SocialTax = 0, TotalTax = 0, NetIncome = grossIncome };
            Taxes actual = (Taxes)_taxService.CalculateTax(grossIncome, charitySpent.GetValueOrDefault());
            AssertByPropertiesAndPass(expected, actual);
        }

        private static void AssertByPropertiesAndPass(Taxes expected, Taxes actual)
        {
            AssertByProperties(actual, expected);
            Assert.Pass();
        }

        private static void AssertByProperties(Taxes actual, Taxes expected)
        {
            Assert.AreEqual(expected.GrossIncome, actual.GrossIncome);
            Assert.AreEqual(expected.IncomeTax, actual.IncomeTax);
            Assert.AreEqual(expected.SocialTax, actual.SocialTax);
            Assert.AreEqual(expected.TotalTax, actual.TotalTax);
            Assert.AreEqual(expected.NetIncome, actual.NetIncome);
            Assert.AreEqual(expected.CharitySpent, actual.CharitySpent);
        }
    }
}