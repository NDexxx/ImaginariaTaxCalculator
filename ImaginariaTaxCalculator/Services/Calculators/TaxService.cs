using ImaginariaTaxCalculator.Helpers.AmountFormatters;
using ImaginariaTaxCalculator.Helpers.Calculators;
using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Interfaces.Calculators;
using ImaginariaTaxCalculator.Models;
using Microsoft.Extensions.Options;

namespace ImaginariaTaxCalculator.Services.Calculators
{
    public class TaxService : ITaxService
    {
        private readonly ISettings<Settings> _settings;
        private readonly IAmountFormatter<CharityAmountFormatter> _charityFormatter;
        private readonly IAmountFormatter<GrossAmountFormatter> _grossIncomeFormatter;
        private readonly IAmountFormatter<SocialTaxAmountFormatter> _socialTaxFormatter;
        private readonly ITaxCalculator<TaxCalculator> _taxCalculator;
        private readonly ITotalTaxCalculator _totalTaxCalculator;
        private readonly INetIncomeCalculator _netIncomeCalculator;

        public TaxService(IOptions<Settings> settings,
                          IAmountFormatter<CharityAmountFormatter> charityFormatter,
                          IAmountFormatter<GrossAmountFormatter> grossIncomeFormatter,
                          IAmountFormatter<SocialTaxAmountFormatter> socialTaxFormatter,
                          ITaxCalculator<TaxCalculator> taxCalculator,
                          ITotalTaxCalculator totalTaxCalculator,
                          INetIncomeCalculator netIncomeCalculator)
        {
            _charityFormatter = charityFormatter;
            _grossIncomeFormatter = grossIncomeFormatter;
            _socialTaxFormatter = socialTaxFormatter;
            _taxCalculator = taxCalculator;
            _totalTaxCalculator = totalTaxCalculator;
            _netIncomeCalculator = netIncomeCalculator;
            _settings = settings.Value;
        }

        /// <summary>
        /// Calculates total amount of taxes and net net income with regards of current jurisdiction tax parameters
        /// </summary>
        /// <param name="grossIncome"></param>
        /// <param name="charitySpent"></param>
        public ITaxes CalculateTax(decimal grossIncome, decimal charitySpent)
        {
            var charitySpentFormatted = _charityFormatter.FormatAmount(grossIncome, charitySpent);
            var grossIncomeFormatted = _grossIncomeFormatter.FormatAmount(grossIncome, charitySpentFormatted);
            var amountForSocialTax = _socialTaxFormatter.FormatAmount(grossIncome, charitySpentFormatted);

            var output = new Taxes
            {
                GrossIncome = grossIncome,
                CharitySpent = charitySpent
            };

            if (grossIncomeFormatted > _settings.MinimumNonTaxableIncome)
            {
                output.IncomeTax = _taxCalculator.CalculateTax(grossIncomeFormatted, _settings.IncomeTaxPercent);
                output.SocialTax = _taxCalculator.CalculateTax(amountForSocialTax, _settings.SocialTaxPercent);
            }

            output.TotalTax = _totalTaxCalculator.CalculateTotalTax(output.IncomeTax, output.SocialTax);
            output.NetIncome = _netIncomeCalculator.CalculateNetIncome(grossIncome, output.TotalTax);

            return output;
        }
    }
}