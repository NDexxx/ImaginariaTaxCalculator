using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.Options;

namespace ImaginariaTaxCalculator.Helpers.AmountFormatters
{
    public class GrossAmountFormatter : IAmountFormatter<GrossAmountFormatter>
    {
        private readonly IOptions<Settings> _settings;

        public GrossAmountFormatter(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Formats gross income based on the minimum non taxable income according to current jurisdiction
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="charitySpent"></param>
        public decimal FormatAmount(decimal grossIncome, decimal charitySpent)
        {
            var formattedGrossIncome = grossIncome - charitySpent;

            if (formattedGrossIncome > _settings.Value.MinimumNonTaxableIncome)
            {
                formattedGrossIncome -= _settings.Value.MinimumNonTaxableIncome;
            }

            return formattedGrossIncome;
        }
    }
}