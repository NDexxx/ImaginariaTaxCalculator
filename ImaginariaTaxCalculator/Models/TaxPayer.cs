using System;
using ImaginariaTaxCalculator.Interfaces;

namespace ImaginariaTaxCalculator.Models
{
    public class TaxPayer : ITaxPayer
    {
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal GrossIncome { get; set; }

        public string SSN { get; set; }

        public decimal? CharitySpent { get; set; }
    }
}