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

        public BookPersistence BookEntityToPersistence(BaseBookEntity bookEntity)
        {
            return new BookPersistence
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                OriginalPrice = bookEntity.UnitPrice,
                EmailProvider = bookEntity.NameProvider,
                Quantity = bookEntity.Cuantity,
                Type = (byte)(int)bookEntity.Type,
                UnitPrice = bookEntity.CurrentPrice,
                Discount = bookEntity.Discount
            };
        }

        public BaseBookEntity BookPersistenceToEntity(BookPersistence bookPersistence, int quantity)
        {
            quantity = quantity == -1 ? (int)bookPersistence.Quantity : quantity;

            BaseBookEntity bookEntity = CreateBookEntity((BaseBookType)bookPersistence.Type, bookPersistence.Id, bookPersistence.Title, (int)bookPersistence.UnitPrice, bookPersistence.EmailProvider, 0, quantity);

            bookEntity.UnitPrice = (int)bookPersistence.UnitPrice;
            bookEntity.CurrentPrice = (float)((float)bookPersistence.UnitPrice * quantity);
            bookEntity.Discount = (float)bookPersistence.Discount;

            return bookEntity;
        }

    }


}
