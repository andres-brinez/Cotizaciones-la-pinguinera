using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;

namespace Sofka.Piguinera.Cotizacion.Models.Factories
{
    public class BaseBookFactory : IBaseBookFactory
    {

        public BaseBookEntity Create(BaseBookInputDTO payload)
        {
            var booksChildren = new Dictionary<BaseBookType, BaseBookEntity>
            {
                {BaseBookType.Book, new BookEntity(payload.Id,payload.Title,payload.OriginalPrice,payload.NameProvider,payload.Seniority,payload.Cuantity)},
                {BaseBookType.Novel, new NovelEntity(payload.Id,payload.Title,payload.OriginalPrice,payload.NameProvider,payload.Seniority,payload.Cuantity)},
            };


            var Book = booksChildren[payload.Type];

            if (Book == null)
            {
                throw new Exception("Tipo de libro no válido");
            }

            return Book;



            
        }
          
    }
}
