using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface ITotalPriceQuotesService
    {

        BooksPurcheseOutputDTO CalculateTotalPriceQuotes(List<InformationInputDto> payload);


    }
}
