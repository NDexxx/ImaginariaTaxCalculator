namespace ImaginariaTaxCalculator.Interfaces
{
    public interface ITaxService
    {
        public ITaxes CalculateTax(decimal grossIncome, decimal charitySpent);
    }
}