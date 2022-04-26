using ImaginariaTaxCalculator.Interfaces;

namespace ImaginariaTaxCalculator.Models
{
    public class Taxes : ITaxes
    {
        public decimal GrossIncome { get; set; }

        public decimal? CharitySpent { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal SocialTax { get; set; }

        public decimal TotalTax { get; set; }

        public decimal NetIncome { get; set; }
    }
}