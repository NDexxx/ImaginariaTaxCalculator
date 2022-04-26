using ImaginariaTaxCalculator.Interfaces.Calculators;

namespace ImaginariaTaxCalculator.Helpers.Calculators
{
    public class TotalTaxCalculator : ITotalTaxCalculator
    {
        /// <summary>
        /// Calculates total tax by summing up income tax and social tax
        /// </summary>
        /// <param name="incomeTax"></param>
        /// <param name="socialTax"></param>
        /// <returns></returns>
        public decimal CalculateTotalTax(decimal incomeTax, decimal socialTax)
        {
            return incomeTax + socialTax;
        }
    }
}