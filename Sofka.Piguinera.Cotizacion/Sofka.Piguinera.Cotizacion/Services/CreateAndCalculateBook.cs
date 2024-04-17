using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;

namespace Sofka.Piguinera.Cotizacion.Services
{
    public class CreateAndCalculateBook
    {

        private readonly IBaseBookFactory _baseBookFactory;

        public CreateAndCalculateBook(IBaseBookFactory baseBookFactory)
        {
            _baseBookFactory = baseBookFactory;
        }

        public BaseBookEntity Entity(BookPersistence bookPersistence, int quantity)
        {
            quantity = quantity == -1 ? (int)bookPersistence.Quantity : quantity;

            BaseBookEntity bookEntity = _baseBookFactory.CreateBookEntity((BaseBookType)bookPersistence.Type, bookPersistence.Id, bookPersistence.Title, (int)bookPersistence.UnitPrice, bookPersistence.NameProvider, (int)bookPersistence.Seniority, quantity);

            bookEntity.OriginalPrice = (int)bookPersistence.UnitPrice;
            bookEntity.CurrentPrice = (float)((float)bookPersistence.UnitPrice * quantity );
            bookEntity.Discount = (float)bookPersistence.Discount;


            return bookEntity;
        }

    }
}
