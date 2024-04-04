using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

       Task<BaseBookOutputDTO> TotalPricePurchese(BaseBookInputDTO payload);

        BooksPurcheseOutputDTO TotalPricePurcheses(List<BaseBookInputDTO> payload);

        BookWithBudgeOutputDTO BooksBudget (BookWithBudgeInputDTO payload);

    }
}
