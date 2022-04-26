namespace ImaginariaTaxCalculator.Interfaces.Calculators
{
    public interface ITaxCalculator<T>
    {
        public decimal CalculateTax(decimal amountToTax, decimal taxPercent);
    }
}