using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class TotalPriceQuotesServiceTest
    {
        private readonly Mock<IDatabase> _databaseMock;
        private readonly ITotalPriceQuotesService service;

        public TotalPriceQuotesServiceTest()
        {
            var data = new List<BookPersistence>().AsQueryable();

            var mockSet = new Mock<DbSet<BookPersistence>>();
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<BookPersistence>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _databaseMock = new Mock<IDatabase>();
            _databaseMock.Setup(database => database.Books).Returns(mockSet.Object);

            service = new TotalPriceQuotesService(_databaseMock.Object);
        }

        public async void ShouldReturnCorrectType_WhenCalledWithValidInput()
        {
            var informationInputDtoList = new List<InformationInputDto>
            {
                new InformationInputDto("1", 5),
                new InformationInputDto("2", 10)
            };

            _databaseMock
              .Setup(database => database.SaveAsync())
              .Returns(Task.FromResult(true));

            var result = service.CalculateTotalPriceQuotes(informationInputDtoList);

            Assert.NotNull(result);
            Assert.IsType<BooksPurcheseOutputDTO>(result);

            _databaseMock.Verify(database => database.Books.AddAsync(It.IsAny<BookPersistence>(), default), Times.Once);
            _databaseMock.Verify(database => database.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CalculateTotalPriceQuotes_WhenDatabaseSaveFails()
        {
            var informationInputDtoList = new List<InformationInputDto>
            {
                new InformationInputDto("1", 5),
                new InformationInputDto("2", 10)
            };

            _databaseMock
                .Setup(database => database.SaveAsync())
               .Throws(new Exception("Database error"));

            var result = service.CalculateTotalPriceQuotes(informationInputDtoList);

            Assert.Empty(result.Books);
        }

        [Fact]
        public async Task CalculateTotalPriceQuotes_ShouldHandleEmptyList()
        {
            var informationInputDtoList = new List<InformationInputDto>();

            var result = service.CalculateTotalPriceQuotes(informationInputDtoList);

            Assert.Empty(result.Books);

        }
    }
}
