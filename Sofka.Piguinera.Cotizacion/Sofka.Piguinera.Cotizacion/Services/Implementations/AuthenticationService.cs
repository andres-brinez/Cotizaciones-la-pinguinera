using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services.Implementations
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IDataBaseService _databaseService;

        public AuthenticationService(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        public UserPersistence GetUser(string email)
        {
            UserPersistence user = _databaseService.GetUser(email);
            return user;
        }

        public async Task<bool> RegisterUser(RegisterUserInputDTO user)
        {

            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserName))
            {
                return false;
            }
            else
            {
                UserPersistence newUser = new UserPersistence(user.Email, user.Password, user.UserName);

                bool isSave = await _databaseService.AddUserAsync(newUser);

                return isSave;

            }

           
        }

        public async Task<bool> LoginUser(LoginUserInputDTO InformationUser)
        {
            UserPersistence user =GetUser(InformationUser.Email);

           
            if (user != null)
            {
                // Este método toma la contraseña sin hashear y la contraseña hasheada, hashea la contraseña sin hashear con la sal de la contraseña hasheada y luego compara los dos hashes.
                bool isEqualsPassword = BCrypt.Net.BCrypt.Verify(InformationUser.Password, user.Password);

                if (isEqualsPassword)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
