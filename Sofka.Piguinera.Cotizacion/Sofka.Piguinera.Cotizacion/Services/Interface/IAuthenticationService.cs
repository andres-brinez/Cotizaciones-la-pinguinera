using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface IAuthenticationService
    {

        Task<bool> RegisterUser(RegisterUserInputDTO user);
        UserPersistence GetUser(string email);
        Task<bool> LoginUser(LoginUserInputDTO user);




    }
}
