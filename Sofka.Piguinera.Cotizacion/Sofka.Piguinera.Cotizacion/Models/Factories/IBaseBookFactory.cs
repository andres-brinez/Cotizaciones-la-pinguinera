using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;

namespace Sofka.Piguinera.Cotizacion.Models.Factories
{
    public interface IBaseBookFactory
    {

        BaseBook Create(BaseBookDTO payload);

    }
}
