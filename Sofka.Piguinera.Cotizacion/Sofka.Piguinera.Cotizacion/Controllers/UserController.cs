using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Sofka.Piguinera.Cotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Description("Controller for handling user operations.")]

    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidator<LoginUserInputDTO> _loginValidator;
        private readonly IValidator<RegisterUserInputDTO> _registerValidator;

        public UserController(IAuthenticationService authenticationService, IValidator<LoginUserInputDTO> loginValidator, IValidator<RegisterUserInputDTO> registerValidator)
        {
            _authenticationService = authenticationService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("Register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Register a new user", Description = "This method takes a RegisterUserInputDTO object as input, validates it, and then registers the user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string indicating that the user was registered successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null or validation fails.")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserInputDTO user)
        {

            var validationResult = await _registerValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            else
            {
                var existingUser = _authenticationService.GetUser(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new { error = "El usuario ya existe" });
                }

                var result = await _authenticationService.RegisterUser(user);

                if (!result)
                {
                    return BadRequest(new { error = "Error al registrar el usuario" });
                }

                return Ok("Usuario registrado exitosamente");

            }
        }

        [HttpPost("Login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Login a user", Description = "This method takes a LoginUserInputDTO object as input, validates it, and then logs in the user.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a string indicating that the user was logged in successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "If the item is null or validation fails.")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserInputDTO user)
        {
            var validationResult = await _loginValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            else {

                var result = await _authenticationService.LoginUser(user);

                if (!result)
                {
                    return BadRequest(new { error = "Correo electrónico o contraseña incorrectos" });
                }

                return Ok("Inicio de sesión exitoso");

            }   
        }
    }
}
