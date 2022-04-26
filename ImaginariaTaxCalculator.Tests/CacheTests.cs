using ImaginariaTaxCalculator.Helpers;
using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ImaginariaTaxCalculator.Tests
{
    public class CacheTests
    {
        private ITaxesCacheHelper _cacheHelper;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new();
            services.AddMemoryCache();
            services.AddScoped<ITaxesCacheHelper, TaxesCacheHelper>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _cacheHelper = serviceProvider.GetService<ITaxesCacheHelper>();
        }

        [Test]
        public void Should_Return_False_With_Non_Existing_Key()
        {
            string key = "123456";
            bool expected = false;
            bool actual = _cacheHelper.DoesKeyExists(key);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Return_True_With_Key_And_Entity_Added()
        {
            string key = "123456";
            _cacheHelper.SetItem(key, new Taxes() { CharitySpent = 1, GrossIncome = 1, IncomeTax = 1, NetIncome = 1, SocialTax = 1, TotalTax = 1 });
            bool expected = true;
            bool actual = _cacheHelper.DoesKeyExists(key);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Built_Keys_Should_Be_The_Same()
        {
            var inputModel = new TaxPayer() { CharitySpent = 100, GrossIncome = 1000, SSN = "12345" };

            string expected = $"{inputModel.SSN}-{inputModel.GrossIncome}-{inputModel.CharitySpent}";
            string actual = _cacheHelper.BuildKey(inputModel);

            Assert.AreEqual(expected, actual);
        }
    }
}