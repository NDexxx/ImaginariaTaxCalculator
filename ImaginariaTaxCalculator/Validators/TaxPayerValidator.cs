using System;
using FluentValidation;
using ImaginariaTaxCalculator.Helpers;
using ImaginariaTaxCalculator.Interfaces.Validators;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.Options;

namespace ImaginariaTaxCalculator.Validators
{
    public class TaxPayerValidator : AbstractValidator<TaxPayer>
    {
        public TaxPayerValidator(IOptions<Settings> settings, IValidationHelper<RegexValidationHelper> _regexValidationHelper)
        {
            RuleFor(r => r.FullName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(m => _regexValidationHelper.IsStringOnlyWithLettersAndSpace(m)).WithMessage("Name must contain only letters")
                .Must(m => _regexValidationHelper.IsStringFromAtLeastTwoWords(m)).WithMessage("You must provide at least two names")
                .Length(3, 50);

            RuleFor(r => r.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(new DateTime(1899, 1, 1).Date).WithMessage("At this age you should be free from taxes :D")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-12).Date).WithMessage("Please add a valid age");

            RuleFor(r => r.SSN)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .MinimumLength(settings.Value.SSNMinLength)
                .MaximumLength(settings.Value.SSNMaxLength)
                .Must(m => _regexValidationHelper.IsStringOnlyWithDigits(m)).WithMessage("SSN must contains only digits");

            RuleFor(r => r.GrossIncome)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0);

            RuleFor(r => r.CharitySpent)
               .Cascade(CascadeMode.Stop)
               .GreaterThanOrEqualTo(0);
        }
    }
}