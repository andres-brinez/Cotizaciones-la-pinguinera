using FluentValidation;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.Input
{
    public class LoginUserInputDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginUserInputDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public class LoginUserInputDTOValidator : AbstractValidator<LoginUserInputDTO>
        {
            public LoginUserInputDTOValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El correo electrónico es requerido");
                RuleFor(x => x.Email).EmailAddress().WithMessage("El correo electrónico no tiene un formato válido");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es requerida");
            }
        }
    }
}
