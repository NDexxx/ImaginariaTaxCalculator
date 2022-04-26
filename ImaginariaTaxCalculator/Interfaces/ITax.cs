namespace ImaginariaTaxCalculator.Interfaces
{
    public interface ITax
    {
        public decimal IncomeTax { get; }
        public decimal SocialTax { get; }
        public decimal TotalTax { get; }
    }
}