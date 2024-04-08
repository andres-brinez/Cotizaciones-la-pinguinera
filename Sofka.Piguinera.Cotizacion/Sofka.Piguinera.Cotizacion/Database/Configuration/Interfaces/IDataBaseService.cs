using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces
{
    public interface IDataBaseService
    {

        BookPersistence GetBookById(string id);
        Task<List<BookPersistence>> GetAllBooksAsync();
        Task<List<BookPersistence>> GetBooksByCategoryAsync(int category);
        Task<bool> AddBookAsync(BookPersistence book);
        Task<bool> UpdateBookAsync(BookPersistence book);
        Task<bool> DeleteBookAsync(string id);

    }
}
