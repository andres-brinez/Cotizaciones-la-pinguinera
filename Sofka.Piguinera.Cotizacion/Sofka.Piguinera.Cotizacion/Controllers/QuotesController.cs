using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;
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
        private readonly IBooksBudgetService _quotesService;
        private readonly ITotalPriceQuotationService _totalPriceQuotationService;
        private readonly ITotalPriceQuotesService _totalPriceQuotesService;
        private readonly IBooksService _booksService;

        private readonly IValidator<BaseBookInputDTO> _validator;
        private readonly IValidator<BookWithBudgeInputDTO> _validatorBudget;
        private readonly IValidator<InformationInputDto> _validatorInformation;

        public QuotesController(IBooksBudgetService quotesService, IValidator<BaseBookInputDTO> validator, IValidator<BookWithBudgeInputDTO> validatorBudget, IValidator<InformationInputDto> validatorInformation, ITotalPriceQuotationService totalPriceQuotationService, ITotalPriceQuotesService totalPriceQuotesService, IBooksService booksService)
        {
            _quotesService = quotesService;
            _totalPriceQuotesService = totalPriceQuotesService;
            _totalPriceQuotationService = totalPriceQuotationService;


            _validator = validator;
            _validatorBudget = validatorBudget;
            _validatorInformation = validatorInformation;
            _booksService = booksService;
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

            try
            {
                var result = await _totalPriceQuotationService.CalculateTotalPriceQuotation(payload);

                // validacion
                if (result == null)
                {
                    return BadRequest(new {error= "Error al guardar en la base de datos" });
                }

                return Ok(result);
            }

            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null && (sqlException.Number == 2627 || sqlException.Number == 2601))
                {
                    return Conflict(new { error = "A book with the same ID already exists." });

                }
                else
                {
                    return BadRequest(new { error = "Failed to add book." });

                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while adding the book." });
            }
        }
        
        [HttpPost("CalculateBooksPay")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BooksPurcheseOutputDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Calculate the total price of multiple books", Description = "This method takes a list of BaseBookDTO objects as input, validates each one, and then calculates the total price of all the books.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string with the total price of all the books, including details about each book, its discount, and its new price.", typeof(BooksPurcheseOutputDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]
        public async Task<ActionResult> CalculateTotalPriceBook([FromBody, SwaggerParameter("The list of book details.", Required = true)] List<InformationInputDto> payload)
        {
            foreach (var item in payload)
            {
                var validationResult = await _validatorInformation.ValidateAsync(item);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
            }

            var result = _totalPriceQuotesService.CalculateTotalPriceQuotes(payload);

            if (result == null)
            {
                return BadRequest(new { error = "Error al guardar en la base de datos" });
            }

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

            if (result == null)
            {
                return BadRequest(new { error = "Error al guardar en la base de datos" });
            }

            return Ok(result);
        }


        [HttpGet("GetAllBooks"),Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<BookPersistence>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get all books", Description = "This method retrieves all books from the database.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of all books in the database.", typeof(List<BookPersistence>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null")]
        public async Task<ActionResult> GetAllBooks()
        {
            var result = _booksService.GetAllBooks();

            if (result == null)
            {
                return BadRequest(new { error = "Error al obtener los lisbros" });
            }

            return Ok(result);
        }
    }
}
