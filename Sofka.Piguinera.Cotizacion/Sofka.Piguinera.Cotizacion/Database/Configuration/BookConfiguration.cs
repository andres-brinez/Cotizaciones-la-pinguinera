using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Database.Configuration
{
    public class BookConfiguration
    {
        public  BookConfiguration(EntityTypeBuilder<BookPersistence> entityBuilder)
        {
            entityBuilder.HasKey(entity => entity.Id);
            entityBuilder.Property(entity => entity.Title).IsRequired();
            entityBuilder.Property(entity => entity.NameProvider).IsRequired();
            entityBuilder.Property(entity => entity.Seniority).IsRequired();
            entityBuilder.Property(entity => entity.OriginalPrice).IsRequired();
            entityBuilder.Property(entity => entity.Quantity).IsRequired();
            entityBuilder.Property(entity => entity.Type).IsRequired();
            entityBuilder.Property(entity => entity.UnitPrice).IsRequired();
            entityBuilder.Property(entity => entity.Discount).IsRequired();
        }
    }
}