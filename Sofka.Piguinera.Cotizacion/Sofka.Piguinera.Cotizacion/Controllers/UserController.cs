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

        public UserController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserInputDTO user)
        {
            var existingUser = _authenticationService.GetUser(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new { error="El usuario ya existe"});
            }

            var result = await _authenticationService.RegisterUser(user);

            if (!result)
            {
                return BadRequest(new { error = "Error al registrar el usuario" });
            }

            return Ok("Usuario registrado exitosamente");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserInputDTO user)
        {
            var result = await _authenticationService.LoginUser(user);

            if (!result)
            {
                return BadRequest(new { error = "Correo electrónico o contraseña incorrectos" });
            }

            return Ok("Inicio de sesión exitoso");
        }
    }
}
