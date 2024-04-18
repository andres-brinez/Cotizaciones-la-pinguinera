using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Services.Interface
{
    public interface IBooksService
    {

        List<BookPersistence> GetAllBooks();

    }
}
