using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.DesignPattern.Factories
{
    public interface IBaseBookFactory
    {

        BaseBookEntity CreateBookEntity(BaseBookType type, string id, string title, int originalPrice, string nameProvider, int seniority, int quantity);
        BookPersistence BookEntityToPersistence(BaseBookEntity bookEntity);
        BaseBookEntity BookPersistenceToEntity(BookPersistence bookPersistence, int quantity);

    }
}
