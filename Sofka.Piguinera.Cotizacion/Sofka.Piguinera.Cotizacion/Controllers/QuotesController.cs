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

        public QuotesController(IQuotesService quotesService, IValidator<BaseBookDTO> validator)
        {
            _quotesService = quotesService;
            _validator = validator;
        }
        
        [HttpPost("CalculateBookPay")]
        public async Task<ActionResult> CalculateTotalPriceBook( BaseBookDTO payload)
        {

            Console.WriteLine("Calculando precio de un libro");

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


    }
}
