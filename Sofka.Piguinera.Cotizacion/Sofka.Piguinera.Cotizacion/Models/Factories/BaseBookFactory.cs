using Sofka.Piguinera.Cotizacion.Models.DTOS;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Factories
{
    public class BaseBookFactory : IBaseBookFactory
    {

        public BaseBook Create(BaseBookDTO payload)
        {
            var booksChildren = new Dictionary<BaseBookType, BaseBook>
            {
                {BaseBookType.Book, new Book(payload.Title,payload.OriginalPrice,payload.NameProvider,payload.Seniority)},
                {BaseBookType.Novel, new Novel(payload.Title,payload.OriginalPrice,payload.NameProvider,payload.Seniority)},
            };


            Console.WriteLine("Creando un libro ");
            Console.WriteLine("Tipo de libro: " + payload.Type);
            Console.WriteLine("Nombre del proveedor: " + payload.NameProvider);
            Console.WriteLine("Antiguedad del proveedor: " + payload.Seniority);

            var Book = booksChildren[payload.Type];

            if (Book == null)
            {
                throw new Exception("Tipo de libro no válido");
            }

            return Book;



            
        }
          
    }
}
