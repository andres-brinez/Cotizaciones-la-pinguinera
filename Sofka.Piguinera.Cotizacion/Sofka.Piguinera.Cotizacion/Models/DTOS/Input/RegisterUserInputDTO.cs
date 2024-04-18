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

    }
}
