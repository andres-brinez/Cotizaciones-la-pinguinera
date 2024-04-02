using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

        String CalculateTotalPricePurchese(BaseBookDTO payload);

        String CalculateTotalPricePurchese(List<BaseBookDTO> payload);

    }
}
