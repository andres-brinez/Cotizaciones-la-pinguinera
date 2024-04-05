using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Factories;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface ITotalPriceQuotationService
    {

        Task<BaseBookOutputDTO> CalculateTotalPriceQuotation(BaseBookInputDTO payload);

    }
}
