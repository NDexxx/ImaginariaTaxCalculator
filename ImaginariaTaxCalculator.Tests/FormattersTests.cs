using System.IO;
using ImaginariaTaxCalculator.Helpers;
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
    public class FormattersTests
    {
        private IAmountFormatter<CharityAmountFormatter> _charityFormatter;
        private IAmountFormatter<GrossAmountFormatter> _grossAmountFormatter;
        private IAmountFormatter<SocialTaxAmountFormatter> _socialTaxFormatter;

        private Settings _settings;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddScoped<ITaxCalculator<TaxCalculator>, TaxCalculator>();
            services.AddScoped<ITotalTaxCalculator, TotalTaxCalculator>();
            services.AddScoped<INetIncomeCalculator, NetIncomeCalculator>();

            services.AddScoped<IAmountFormatter<CharityAmountFormatter>, CharityAmountFormatter>();
            services.AddScoped<IAmountFormatter<GrossAmountFormatter>, GrossAmountFormatter>();
            services.AddScoped<IAmountFormatter<SocialTaxAmountFormatter>, SocialTaxAmountFormatter>();

            services.AddScoped<ITaxesCacheHelper, TaxesCacheHelper>();

            services.AddScoped<ITaxService, TaxService>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

            services.AddOptions<Settings>().Bind(configuration.GetSection("Settings"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var options = serviceProvider.GetService<IOptions<Settings>>();
            _settings = options.Value;

            _charityFormatter = serviceProvider.GetService<IAmountFormatter<CharityAmountFormatter>>();
            _grossAmountFormatter = serviceProvider.GetService<IAmountFormatter<GrossAmountFormatter>>();
            _socialTaxFormatter = serviceProvider.GetService<IAmountFormatter<SocialTaxAmountFormatter>>();
        }

        [TestCase(4000, 500)]
        public void Charity_Formatter_With_Charity_Above_Max_Limit(decimal grossIncome, decimal charitySpent)
        {
            decimal maxCharityPercent = _settings.MaximumCharityPercentFromGrossIncome;

            decimal expected = grossIncome / maxCharityPercent;
            decimal actual = _charityFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [TestCase(4000, 300)]
        public void Charity_Formatter_With_Charity_Below_Max(decimal grossIncome, decimal charitySpent)
        {
            decimal expected = charitySpent;
            decimal actual = _charityFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [TestCase(4000, 500)]
        public void Gross_Formatter_With_Wage_Above_Min(decimal grossIncome, decimal charitySpent)
        {
            decimal minNonTaxableIncome = _settings.MinimumNonTaxableIncome;

            decimal expected = grossIncome - minNonTaxableIncome - charitySpent;
            decimal actual = _grossAmountFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [TestCase(1000, 500)]
        public void Gross_Formatter_With_Wage_Equals_To_Min(decimal grossIncome, decimal charitySpent)
        {
            decimal minNonTaxableIncome = _settings.MinimumNonTaxableIncome;

            decimal expected = minNonTaxableIncome - charitySpent;
            decimal actual = _grossAmountFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [TestCase(2000, 200)]
        public void Social_Formatter_With_Wage_Below_Max(decimal grossIncome, decimal charitySpent)
        {
            decimal expected = grossIncome - charitySpent - _settings.MinimumNonTaxableIncome;
            decimal actual = _socialTaxFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [Test]
        public void Social_Formatter_With_Wage_Above_Max_Social_Limit()
        {
            decimal grossIncome = _settings.MaximumSocialContributionLimit + 1;
            int charitySpent = 150;

            decimal expected = _settings.MaximumSocialContributionLimit - _settings.MinimumNonTaxableIncome;
            decimal actual = _socialTaxFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        [Test]
        public void Social_Formatter_With_Wage_Below_Max_Social_Limit()
        {
            decimal grossIncome = _settings.MaximumSocialContributionLimit - 1;
            int charitySpent = 150;

            decimal expected = grossIncome - charitySpent - _settings.MinimumNonTaxableIncome;
            decimal actual = _socialTaxFormatter.FormatAmount(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        private static void AssertEqualAndPass(decimal expected, decimal actual)
        {
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }
    }
}