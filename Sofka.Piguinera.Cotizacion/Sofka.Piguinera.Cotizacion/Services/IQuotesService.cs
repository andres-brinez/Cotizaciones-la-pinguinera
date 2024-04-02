using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

        String CalculateTotalPrice(BaseBookDTO payload);

        String CalculateTotalPrice(List<BaseBookDTO> payload);

    }
}
