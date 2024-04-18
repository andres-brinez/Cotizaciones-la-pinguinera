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
            UserPersistence newUser = new UserPersistence(user.Email, user.Password, user.UserName);

            bool isSave= await _databaseService.AddUserAsync(newUser);

            return isSave;
        }

    }
}
