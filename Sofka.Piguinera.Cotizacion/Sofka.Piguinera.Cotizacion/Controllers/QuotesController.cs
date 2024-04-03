using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Sofka.Piguinera.Cotizacion.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Description("Controller for handling book quote operations.")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuotesService _quotesService;
        private readonly IValidator<BaseBookDTO> _validator;
        private readonly IValidator<BookWithBudgetDTO> _validatorBudget;

        public QuotesController(IQuotesService quotesService, IValidator<BaseBookDTO> validator, IValidator<BookWithBudgetDTO> validatorBudget)
        {
            _quotesService = quotesService;
            _validator = validator;
            _validatorBudget = validatorBudget;
        }



         [HttpPost("CalculateBookPay")]
         [SwaggerOperation(Summary = "Calculate the total price of a book", Description = "This method takes a BaseBookDTO object as input, validates it, and then calculates the total price of the book.")]
         [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the total price of the book, including details about the book, its discount, and its new price.")]
        public async Task<ActionResult> CalculateTotalPriceBook([FromBody, SwaggerParameter("The book details.", Required = true)] BaseBookDTO payload)
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
        [SwaggerOperation(Summary = "Calculate the total price of multiple books", Description = "This method takes a list of BaseBookDTO objects as input, validates each one, and then calculates the total price of all the books.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the total price of all the books, including details about each book, its discount, and its new price.")]
        

        public async Task<ActionResult> CalculateTotalPriceBook([FromBody, SwaggerParameter("The list of book details.", Required = true)] List<BaseBookDTO> payload)
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
        [SwaggerOperation(Summary = "Calculate the total price of a book with a budget", Description = "This method takes a BookWithBudgetDTO object as input, validates it, and then calculates the total price of the book within the given budget.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the books that can be bought with the given budget, or a message indicating that no books can be bought with the current budget.")]
        public async Task<ActionResult> CalculateTotalPriceBookBudget([FromBody, SwaggerParameter("The book details with budget.", Required = true)] BookWithBudgetDTO payload)
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
