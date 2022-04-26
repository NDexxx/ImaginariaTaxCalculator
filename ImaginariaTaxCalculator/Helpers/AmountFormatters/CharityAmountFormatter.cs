using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.Options;

namespace ImaginariaTaxCalculator.Helpers.AmountFormatters
{
    public class CharityAmountFormatter : IAmountFormatter<CharityAmountFormatter>
    {
        private readonly IOptions<Settings> _settings;

        public CharityAmountFormatter(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Formats charity amount based on the maximum charity percent according to current jurisdiction
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="charitySpent"></param>
        public decimal FormatAmount(decimal grossIncome, decimal charitySpent)
        {
            var formattedCharity = charitySpent;
            var maxAllowedCharity = grossIncome * (_settings.Value.MaximumCharityPercentFromGrossIncome / 100);

            if (charitySpent > maxAllowedCharity)
                formattedCharity = maxAllowedCharity;

            return formattedCharity;
        }
    }
}