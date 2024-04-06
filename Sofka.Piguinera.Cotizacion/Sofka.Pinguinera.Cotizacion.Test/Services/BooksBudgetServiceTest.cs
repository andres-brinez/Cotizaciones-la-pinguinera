using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class BooksBudgetServiceTest
    {
        private readonly Mock<IDatabase> _databaseMock;
        private readonly IBooksBudgetService service;

        public BooksBudgetServiceTest()
        {
            var data = new List<BookPersistence>().AsQueryable();
            var mockSet = new Mock<DbSet<BookPersistence>>();
            
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _databaseMock = new Mock<IDatabase>();
            _databaseMock.Setup(database => database.Books).Returns(mockSet.Object);

            service = new BooksBudgetService(_databaseMock.Object);
        }


        [Fact]
        public async void ShouldReturnCorrectType_WhenCalledWithValidInput()
        {
            BookWithBudgeInputDTO books = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };

            // Setup mock to return a book when FindAsync is called
            _databaseMock
                .Setup(database => database.Books.FindAsync(It.IsAny<string>()))
                .Returns(ValueTask.FromResult(new BookPersistence()));

            var result = service.BooksBudget(books);

            Assert.NotNull(result);
            Assert.IsType<BookWithBudgeOutputDTO>(result);

        }

        [Fact]
        public async Task CalculateTotalPriceQuotes_ShouldReturnEmptyList_WhenBooksNotFound()
        {
            var informationBooks = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };

            // Simulate that the books are not found in the database
            var mockSet = new Mock<DbSet<BookPersistence>>();

            // Se configura para que se retorne una lista vacía
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Provider).Returns(new List<BookPersistence>().AsQueryable().Provider);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Expression).Returns(new List<BookPersistence>().AsQueryable().Expression);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.ElementType).Returns(new List<BookPersistence>().AsQueryable().ElementType);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.GetEnumerator()).Returns(new List<BookPersistence>().AsQueryable().GetEnumerator());

            _databaseMock.Setup(database => database.Books).Returns(mockSet.Object);

            var result = service.BooksBudget(informationBooks);

            Assert.NotNull(result);
            Assert.Empty(result.Books);
        }


        [Fact]
        public async Task CalculateTotalPriceQuotes_ShouldHandleEmptyList()
        {
            var informationBooks = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { },
                Budget = 0
            };

            var result = service.BooksBudget(informationBooks);

            Assert.Empty(result.Books);

        }

    }
}
