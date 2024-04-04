using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Models.Factories
{
    public interface IBaseBookFactory
    {

        BaseBookEntity Create(BaseBookInputDTO payload);

    }
}
