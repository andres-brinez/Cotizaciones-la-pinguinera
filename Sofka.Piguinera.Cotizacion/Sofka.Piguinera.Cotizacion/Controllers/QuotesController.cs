using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Services;

namespace Sofka.Piguinera.Cotizacion.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuotesService _quotesService;
        private readonly IValidator<BaseBookDTO> _validator ;
        private readonly IValidator<BookWithBudgetDTO> _validatorBudget;



        public QuotesController(IQuotesService quotesService, IValidator<BaseBookDTO> validator, IValidator<BookWithBudgetDTO> validatorBudget)
        {
            _quotesService = quotesService;
            _validator = validator;
            _validatorBudget = validatorBudget;
        }
        
        [HttpPost("CalculateBookPay")]
        public async Task<ActionResult> CalculateTotalPriceBook( BaseBookDTO payload)
        {


            var validationResult = await _validator.ValidateAsync(payload);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = _quotesService.TotalPricePurchese(payload);
            return Ok(result);
        }
        


        [HttpPost("CalculateBooksPay")]
        public async Task<ActionResult> CalculateTotalPriceBook(List<BaseBookDTO> payload)
        {

            foreach (var item in payload)
            {
                var validationResult = await _validator.ValidateAsync(item);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
            }

            var result = _quotesService.TotalPricePurcheses(payload);
            return Ok(result);
        }


        [HttpPost("CalculateBooksBudget")]
        public async Task<ActionResult> CalculateTotalPriceBookBudget(BookWithBudgetDTO payload)
        {

            var validationResult = await _validatorBudget.ValidateAsync(payload);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = _quotesService.BooksBudget(payload);
            return Ok(result);
        }


    }
}
