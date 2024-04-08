using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.DesignPattern.Factories
{
    public class BaseBookFactory : IBaseBookFactory
    {

        public BaseBookEntity CreateBookEntity(BaseBookType type, string id, string title, int originalPrice, string nameProvider, int seniority, int quantity)
        {

            var booksChildren = new Dictionary<BaseBookType, BaseBookEntity>
                {
                    {BaseBookType.Book, new BookEntity(id, title, originalPrice, nameProvider, seniority, quantity)},
                        {BaseBookType.Novel, new NovelEntity(id, title, originalPrice, nameProvider, seniority, quantity)},
                };


            var Book = booksChildren[type];

            if (Book == null)
            {
                throw new Exception("Tipo de libro no válido");
            }

            return Book;

        }

        public BookPersistence CreateBookPersistence(BaseBookEntity bookEntity)
        {
            return new BookPersistence
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                OriginalPrice = bookEntity.OriginalPrice,
                NameProvider = bookEntity.NameProvider,
                Seniority = bookEntity.Seniority,
                Quantity = bookEntity.Cuantity,
                Type = (byte)(int)bookEntity.Type,
                UnitPrice = bookEntity.CurrentPrice,
                Discount = bookEntity.Discount
            };
        }

    }


}
