using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.Options;

namespace ImaginariaTaxCalculator.Helpers.AmountFormatters
{
    public class SocialTaxAmountFormatter : IAmountFormatter<SocialTaxAmountFormatter>
    {
        private readonly IOptions<Settings> _settings;

        public SocialTaxAmountFormatter(IOptions<Settings> settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Formats social contribution amount according to maximual social contribution limit and minimum non taxable income
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="charitySpent"></param>
        public decimal FormatAmount(decimal grossIncome, decimal charitySpent)
        {
            var maxSocialLimit = _settings.Value.MaximumSocialContributionLimit;
            var minNonTaxInc = _settings.Value.MinimumNonTaxableIncome;

            var formattedInput = 0m;
            if (grossIncome > maxSocialLimit)
                formattedInput = maxSocialLimit - minNonTaxInc;
            else if (grossIncome > minNonTaxInc)
                formattedInput = grossIncome - charitySpent - minNonTaxInc;

            return formattedInput;
        }
    }
}