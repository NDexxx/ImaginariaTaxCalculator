namespace ImaginariaTaxCalculator.Interfaces.Calculators
{
    public interface INetIncomeCalculator
    {
        public decimal CalculateNetIncome(decimal grossIncome, decimal totalTax);
    }
}