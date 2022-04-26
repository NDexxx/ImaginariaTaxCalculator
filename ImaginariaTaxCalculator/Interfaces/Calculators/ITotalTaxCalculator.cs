namespace ImaginariaTaxCalculator.Interfaces.Calculators
{
    public interface ITotalTaxCalculator
    {
        public decimal CalculateTotalTax(decimal incomeTax, decimal socialTax);
    }
}