using ImaginariaTaxCalculator.Interfaces.Calculators;

namespace ImaginariaTaxCalculator.Helpers.Calculators
{
    public class TaxCalculator : ITaxCalculator<TaxCalculator>
    {
        /// <summary>
        /// Calculates the tax amount
        /// </summary>
        /// <param name="amountToTax"></param>
        /// <param name="taxPercent"></param>
        public decimal CalculateTax(decimal amountToTax, decimal taxPercent)
        {
            return (amountToTax * (taxPercent / 100));
        }
    }
}