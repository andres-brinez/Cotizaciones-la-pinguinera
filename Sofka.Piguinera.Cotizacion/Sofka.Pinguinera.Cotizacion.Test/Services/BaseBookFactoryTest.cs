using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class BaseBookFactoryTest
    {
        private readonly IBaseBookFactory _factory;

        public BaseBookFactoryTest()
        {
            _factory = new BaseBookFactory();
        }

        [Fact]
        public void CreateBookEntity_ShouldReturnCorrectType_WhenTypeIsValid()
        {
            var type = BaseBookType.Book;
            var id = "1";
            var title = "Title1";
            var originalPrice = 100;
            var nameProvider = "Provider1";
            var seniority = 1;
            var quantity = 5;

            var result = _factory.CreateBookEntity(type, id, title, originalPrice, nameProvider, seniority, quantity);

            Assert.IsType<BookEntity>(result);
        }

     

        [Fact]
        public void BookEntityToPersistence_ShouldReturnCorrectType()
        {
            var bookEntity = new BookEntity("1", "Title1", 100, "Provider1", 1, 5);

            var result = _factory.BookEntityToPersistence(bookEntity);

            Assert.IsType<BookPersistence>(result);
        }

        [Fact]
        public void BookPersistenceToEntity_ShouldReturnCorrectTypeNovel()
        {
            var bookPersistence = new BookPersistence { Id = "1", Title = "Title1", OriginalPrice = 100, EmailProvider = "Provider1", Quantity = 5, Type = 1, UnitPrice = 90, Discount = 10 };
            var quantity = 5;

            var result = _factory.BookPersistenceToEntity(bookPersistence, quantity);

            Assert.IsType<NovelEntity>(result);
        }
        [Fact]
        public void BookPersistenceToEntity_ShouldReturnCorrectTypeBook()
        {
            var bookPersistence = new BookPersistence { Id = "1", Title = "Title1", OriginalPrice = 100, EmailProvider = "Provider1", Quantity = 5, Type = 0, UnitPrice = 90, Discount = 10 };
            var quantity = 5;

            var result = _factory.BookPersistenceToEntity(bookPersistence, quantity);

            Assert.IsType<BookEntity>(result);
        }




    }
}
