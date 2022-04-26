using ImaginariaTaxCalculator.Interfaces;

namespace ImaginariaTaxCalculator.Models
{
    public class Settings : ISettings<Settings>
    {
        public decimal MinimumNonTaxableIncome { get; set; }

        public decimal MaximumSocialContributionLimit { get; set; }

        public decimal MaximumCharityPercentFromGrossIncome { get; set; }

        public decimal IncomeTaxPercent { get; set; }

        public decimal SocialTaxPercent { get; set; }

        public int SSNMinLength { get; set; }

        public int SSNMaxLength { get; set; }
    }
}