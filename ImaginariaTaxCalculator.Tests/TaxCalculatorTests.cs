using ImaginariaTaxCalculator.Helpers.Calculators;
using ImaginariaTaxCalculator.Interfaces.Calculators;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ImaginariaTaxCalculator.Tests
{
    public class TaxCalculatorTests
    {
        private ITaxCalculator<TaxCalculator> _taxCalculator;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddScoped<ITaxCalculator<TaxCalculator>, TaxCalculator>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            _taxCalculator = serviceProvider.GetService<ITaxCalculator<TaxCalculator>>();
        }

        [TestCase(1000, 20)]
        public void Charity_Formatter_With_Charity_Above_Max_Limit2(decimal grossIncome, decimal charitySpent)
        {
            decimal expected = 1000 * 20 / 100;
            decimal actual = _taxCalculator.CalculateTax(grossIncome, charitySpent);

            AssertEqualAndPass(expected, actual);
        }

        private static void AssertEqualAndPass(decimal expected, decimal actual)
        {
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }
    }
}