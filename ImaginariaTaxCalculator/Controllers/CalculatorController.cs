using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ImaginariaTaxCalculator.Interfaces;
using ImaginariaTaxCalculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImaginariaTaxCalculator.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly IValidator<TaxPayer> _taxPayerValidator;
        private readonly ITaxService _taxService;
        private readonly ITaxesCacheHelper _cacheHelper;

        public CalculatorController(IValidator<TaxPayer> taxPayerValidator,
                             ITaxService taxCalculator,
                             ITaxesCacheHelper cacheHelper)
        {
            _taxPayerValidator = taxPayerValidator;
            _taxService = taxCalculator;
            _cacheHelper = cacheHelper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Taxes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ValidationFailure>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Calculate(TaxPayer input)
        {
            ValidationResult validationResults = await _taxPayerValidator.ValidateAsync(input);
            if (!validationResults.IsValid)
                return BadRequest(validationResults.Errors);

            var key = _cacheHelper.BuildKey(input);

            if (_cacheHelper.DoesKeyExists(key))
                return Ok(_cacheHelper.GetItem(key));
            else
            {
                var output = _taxService.CalculateTax(input.GrossIncome, input.CharitySpent.GetValueOrDefault());
                _cacheHelper.SetItem(key, output);
                return Ok(output);
            }
        }
    }
}