using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
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
        private readonly IValidator<BaseBookInputDTO> _validator;
        private readonly IValidator<BookWithBudgeInputDTO> _validatorBudget;

        public QuotesController(IQuotesService quotesService, IValidator<BaseBookInputDTO> validator, IValidator<BookWithBudgeInputDTO> validatorBudget)
        {
            _quotesService = quotesService;
            _validator = validator;
            _validatorBudget = validatorBudget;
        }



        [HttpPost("CalculateBookPay")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BaseBookOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Calculate the total price of a book", Description = "This method takes a BaseBookDTO object as input, validates it, and then calculates the total price of the book.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the total price of the book, including details about the book, its discount, and its new price.", typeof(BaseBookOutputDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]
        public async Task<ActionResult> CalculateTotalPriceBook([FromBody, SwaggerParameter("The book details.", Required = true)] BaseBookInputDTO payload)
        {

            var validationResult = await _validator.ValidateAsync(payload);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _quotesService.TotalPricePurchese(payload);
            // validacion
            if (result == null)
            {
                return BadRequest("Error al guardar en la base de datos");
            }

            return Ok(result);
        }


        [HttpPost("CalculateBooksPay")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BooksPurcheseOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Calculate the total price of multiple books", Description = "This method takes a list of BaseBookDTO objects as input, validates each one, and then calculates the total price of all the books.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the total price of all the books, including details about each book, its discount, and its new price.", typeof(BooksPurcheseOutputDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]

        public async Task<ActionResult> CalculateTotalPriceBook([FromBody, SwaggerParameter("The list of book details.", Required = true)] List<BaseBookInputDTO> payload)
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(BookWithBudgeOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Calculate the total price of a book with a budget", Description = "This method takes a BookWithBudgetDTO object as input, validates it, and then calculates the total price of the book within the given budget.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the books that can be bought with the given budget, or a message indicating that no books can be bought with the current budget.", typeof(BookWithBudgeOutputDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]

        public async Task<ActionResult> CalculateTotalPriceBookBudget([FromBody, SwaggerParameter("The book details with budget.", Required = true)] BookWithBudgeInputDTO payload)
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
