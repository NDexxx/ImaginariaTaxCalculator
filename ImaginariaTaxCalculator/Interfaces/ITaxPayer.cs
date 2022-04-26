using System;

namespace ImaginariaTaxCalculator.Interfaces
{
    public interface ITaxPayer : ICharitySpent, IGrossIncome
    {
        public string FullName { get; }
        public DateTime DateOfBirth { get; }
        public string SSN { get; }
    }
}