using System.Text.RegularExpressions;
using ImaginariaTaxCalculator.Interfaces.Validators;

namespace ImaginariaTaxCalculator.Helpers
{
    public class RegexValidationHelper : IValidationHelper<RegexValidationHelper>
    {
        public bool IsStringOnlyWithDigits(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]+$");
        }

        public bool IsStringOnlyWithLettersAndSpace(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z ]+$");
        }

        public bool IsStringFromAtLeastTwoWords(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z]+(?:\s[A-Za-z]+)+$");
        }
    }
}