using Microsoft.EntityFrameworkCore;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces
{
    public interface IDatabase
    {

        DbSet<BookPersistence> Books { get; set; }
        Task<bool> SaveAsync();

    }
}
