using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
