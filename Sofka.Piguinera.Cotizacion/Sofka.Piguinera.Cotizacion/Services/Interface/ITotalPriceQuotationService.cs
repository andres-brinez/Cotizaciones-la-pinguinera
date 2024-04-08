using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface ITotalPriceQuotationService
    {

        Task<BaseBookOutputDTO> CalculateTotalPriceQuotation(BaseBookInputDTO payload);

    }
}
