using Microsoft.EntityFrameworkCore;
using Sofka.Piguinera.Cotizacion.Database.Configuration;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.Persistence;


namespace Sofka.Piguinera.Cotizacion.Database
{
    public class Database(DbContextOptions options) : DbContext(options),IDatabase
    {
        public DbSet<BookPersistence> Books { get; set; } // Define the table in the database
        public DbSet<UserPersistence> Users { get; set; } // Define the table in the database

        public async Task<bool> SaveAsync() // verifica que se haya hecho un cambio en la base de datos
        {
            return await SaveChangesAsync() > 0; 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityConfiguration(modelBuilder);

        }

        private void EntityConfiguration(ModelBuilder modelBuilder)
        {
            new BookConfiguration(modelBuilder.Entity<BookPersistence>());
            new UserConfiguration(modelBuilder.Entity<UserPersistence>()); 

        }
    }

}