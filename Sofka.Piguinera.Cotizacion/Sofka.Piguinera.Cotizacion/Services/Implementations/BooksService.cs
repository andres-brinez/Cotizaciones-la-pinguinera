using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Services.Implementations
{
    public class BooksService : IBooksService
    {
        private readonly IDataBaseService _databaseService;

        public BooksService(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<BookPersistence> GetAllBooks()
        {
            return _databaseService.GetAllBooksAsync().Result;
        }

    }
}
