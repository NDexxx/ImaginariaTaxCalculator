using ImaginariaTaxCalculator.Interfaces.Calculators;

namespace ImaginariaTaxCalculator.Helpers.Calculators
{
    public class NetIncomeCalculator : INetIncomeCalculator
    {
        public decimal CalculateNetIncome(decimal grossIncome, decimal totalTax)
        {
            return grossIncome - totalTax;
        }
    }
}