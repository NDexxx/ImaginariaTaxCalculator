namespace ImaginariaTaxCalculator.Interfaces
{
    public interface ISettings<T>
    {
        public decimal MinimumNonTaxableIncome { get; }
        public decimal MaximumSocialContributionLimit { get; }
        public decimal MaximumCharityPercentFromGrossIncome { get; }
        public decimal IncomeTaxPercent { get; }
        public decimal SocialTaxPercent { get; }
        public int SSNMinLength { get; }
        public int SSNMaxLength { get; }
    }
}