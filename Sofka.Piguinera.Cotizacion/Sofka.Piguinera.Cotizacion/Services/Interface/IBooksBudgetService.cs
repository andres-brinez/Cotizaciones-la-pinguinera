using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface IBooksBudgetService
    {


        BookWithBudgeOutputDTO BooksBudget(BookWithBudgeInputDTO payload);

    }
}
