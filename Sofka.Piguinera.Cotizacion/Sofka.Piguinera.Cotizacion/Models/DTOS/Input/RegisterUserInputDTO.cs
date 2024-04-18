using FluentValidation;

namespace Sofka.Piguinera.Cotizacion.Models.DTOS.Input
{
    public class RegisterUserInputDTO
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }


        public RegisterUserInputDTO(string email, string password, string userName)
        {
            Email = email;
            Password = password;
            UserName = userName;
        }

        public class RegisterUserInputDTOValidator : AbstractValidator<RegisterUserInputDTO>
        {
            public RegisterUserInputDTOValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El correo electrónico es requerido");
                RuleFor(x => x.Email).EmailAddress().WithMessage("El correo electrónico no tiene un formato válido");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es requerida");
                RuleFor(x => x.UserName).NotEmpty().WithMessage("El nombre de usuario es requerido");
            }
        }

    }
}
