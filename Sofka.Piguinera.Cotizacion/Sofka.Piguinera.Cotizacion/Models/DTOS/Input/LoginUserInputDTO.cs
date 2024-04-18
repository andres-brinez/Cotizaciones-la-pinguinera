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
    }
}
