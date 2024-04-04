using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

       Task<BaseBookOutputDTO> TotalPricePurchese(BaseBookInputDTO payload);

        BooksPurcheseOutputDTO TotalPricePurcheses(List<InformationInputDto> payload);

        BookWithBudgeOutputDTO BooksBudget (BookWithBudgeInputDTO payload);

    }
}
