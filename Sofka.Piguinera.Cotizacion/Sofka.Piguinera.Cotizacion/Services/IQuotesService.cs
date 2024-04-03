using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public interface IQuotesService
    {

        BaseBookOutputDTO TotalPricePurchese(BaseBookInputDTO payload);

        String TotalPricePurcheses(List<BaseBookInputDTO> payload);

        string BooksBudget (BookWithBudgeInputDTO payload);

    }
}
