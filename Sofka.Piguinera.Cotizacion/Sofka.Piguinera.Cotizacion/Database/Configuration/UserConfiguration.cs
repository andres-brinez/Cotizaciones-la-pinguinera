using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Database.Configuration
{
    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<UserPersistence> entityBuilder)
        {
            entityBuilder.HasKey(entity => entity.Email);
            entityBuilder.Property(entity => entity.Password).IsRequired();
            entityBuilder.Property(entity => entity.UserName).IsRequired();
            entityBuilder.Property(entity => entity.RegistrationDate).HasDefaultValueSql("GETDATE()");
            entityBuilder
                .Property(entity => entity.Seniority)
                .ValueGeneratedOnAddOrUpdate() // Indica que el valor de la propiedad se generará automáticamente cuando se inserte o actualice la entidad.
                .HasComputedColumnSql();  // Indica que la propiedad es una columna calculada en la base de datos.
        }

    }
}
