using Microsoft.EntityFrameworkCore;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Database
{
    public class DataBaseService : IDataBaseService
    {

        private readonly IDatabase _database;

        public DataBaseService(IDatabase database)
        {
            _database = database;
        }

        private T ExecuteDbOperation<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }


        public BookPersistence GetBookById(string id)
        {
            return ExecuteDbOperation(() => _database.Books.FirstOrDefault(b => b.Id == id));
        }

        public Task<List<BookPersistence>> GetAllBooksAsync()
        {
            return ExecuteDbOperation(() => _database.Books.ToListAsync());
        }

        public Task<List<BookPersistence>> GetBooksByCategoryAsync(int category)
        {
            return ExecuteDbOperation(() => _database.Books.Where(b => b.Type == category).ToListAsync());
        }

        public Task<bool> AddBookAsync(BookPersistence book)
        {
            return ExecuteDbOperation(async () =>
            {
                await _database.Books.AddAsync(book);
                return await _database.SaveAsync();
            });
        }

        public Task<bool> UpdateBookAsync(BookPersistence book)
        {
            return ExecuteDbOperation(async () =>
            {
                _database.Books.Update(book);
                return await _database.SaveAsync();
            });
        }

        public Task<bool> DeleteBookAsync(string id)
        {
            return ExecuteDbOperation(async () =>
            {
                var book = _database.Books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    _database.Books.Remove(book);
                    return await _database.SaveAsync();
                }
                return false;
            });
        }




    }
}
