namespace ImaginariaTaxCalculator.Interfaces.Validators
{
    public interface IValidationHelper<T>
    {
        public bool IsStringOnlyWithDigits(string input);

        public bool IsStringOnlyWithLettersAndSpace(string input);

        public bool IsStringFromAtLeastTwoWords(string input);
    }
}