namespace ImaginariaTaxCalculator.Interfaces
{
    public interface IAmountFormatter<T>
    {
        public decimal FormatAmount(decimal grossIncome, decimal charitySpent);
    }
}