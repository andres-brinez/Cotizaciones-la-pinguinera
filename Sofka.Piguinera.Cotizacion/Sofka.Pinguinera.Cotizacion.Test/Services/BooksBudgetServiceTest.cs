using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Implementations;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class BooksBudgetServiceTest
    {
        private readonly Mock<IBaseBookFactory> _baseBookFactoryMock;
        private readonly Mock<IDataBaseService> _databaseServiceMock;
        private readonly BooksBudgetService _service;

        public BooksBudgetServiceTest()
        {
            _baseBookFactoryMock = new Mock<IBaseBookFactory>();
            _databaseServiceMock = new Mock<IDataBaseService>();
            _service = new BooksBudgetService(_baseBookFactoryMock.Object, _databaseServiceMock.Object);
        }

        [Fact]
        public void BooksBudget_ShouldReturnCorrectValue_WhenBooksExist()
        {
            var input = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };
            var bookPersistence = new BookPersistence { Id = "1", Title = "Title1", EmailProvider = "user1@test.com", OriginalPrice = 100, Quantity = 5, Type = 1, UnitPrice = 90, Discount = 10 };
            var bookOutputDto = new BaseBookOutputDTO("Title1", 0, 90, 10, 5);
            var expectedOutput = new BookWithBudgeOutputDTO(new List<BaseBookOutputDTO> { bookOutputDto }, 1000);

            _baseBookFactoryMock.Setup(factory => factory.BookPersistenceToEntity(bookPersistence, It.IsAny<int>()));

            var result = _service.BooksBudget(input);
            Assert.NotNull(result);
            Assert.IsType<BookWithBudgeOutputDTO>(result);
        }

        [Fact]
        public void BooksBudget_ShouldReturnEmptyList_WhenBooksDoNotExist()
        {
            var input = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };

            _databaseServiceMock.Setup(service => service.GetBookById(It.IsAny<string>())).Returns((BookPersistence)null);

            var result = _service.BooksBudget(input);

            Assert.Empty(result.Books);
        }

        [Fact]
        public void BooksBudget_ShouldHandleEmptyList()
        {
            // Arrange
            var input = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string>(),
                Budget = 0
            };

            var result = _service.BooksBudget(input);
            Assert.Empty(result.Books);
        }
    }
}