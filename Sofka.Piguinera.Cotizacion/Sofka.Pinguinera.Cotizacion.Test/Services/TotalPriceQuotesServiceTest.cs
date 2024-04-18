using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Implementations;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class TotalPriceQuotesServiceTest
    {

        private readonly Mock<IBaseBookFactory> _baseBookFactoryMock;
        private readonly Mock<IDataBaseService> _databaseServiceMock;
        private readonly TotalPriceQuotesService _service;

        public TotalPriceQuotesServiceTest()
        {
            _baseBookFactoryMock = new Mock<IBaseBookFactory>();
            _databaseServiceMock = new Mock<IDataBaseService>();
            _service = new TotalPriceQuotesService(_baseBookFactoryMock.Object, _databaseServiceMock.Object);
        }

        [Fact]
        public void CalculateTotalPriceQuotes_ShouldReturnCorrectValue_WhenBooksExist()
        {
            var input = new List<InformationInputDto>
            {
                new InformationInputDto("1", 5),
                new InformationInputDto("2", 10)
            };
            var bookPersistence = new BookPersistence { Id = "1", Title = "Title1", EmailProvider = "user1@test.com", OriginalPrice = 100, Quantity = 5, Type = 1, UnitPrice = 90, Discount = 10 };
            var bookOutputDto = new BaseBookOutputDTO("Title1", 0, 90, 10, 5);
            var expectedOutput = new BooksPurcheseOutputDTO(new List<BaseBookOutputDTO> { bookOutputDto });
            BookEntity bookEntity = new BookEntity("1","Title1", 100, "user1@test.com", 10, 5);
            _databaseServiceMock.Setup(service => service.GetBookById(It.IsAny<string>())).Returns(bookPersistence);
            _baseBookFactoryMock.Setup(factory => factory.BookPersistenceToEntity(bookPersistence, It.IsAny<int>())).Returns(bookEntity);

            var result = _service.CalculateTotalPriceQuotes(input);

            Assert.True(result.Books is List<BaseBookOutputDTO>);
            Assert.NotEmpty(result.Books);
        }
        [Fact]
        public void CalculateTotalPriceQuotes_ShouldReturnEmptyList_WhenBooksDoNotExist()
        {
            var input = new List<InformationInputDto>
        {
            new InformationInputDto("1", 5),
            new InformationInputDto("2", 10)
        };

            _databaseServiceMock.Setup(service => service.GetBookById(It.IsAny<string>())).Returns((BookPersistence)null);

            var result = _service.CalculateTotalPriceQuotes(input);

            Assert.Empty(result.Books);
        }

        [Fact]
        public void CalculateTotalPriceQuotes_ShouldHandleEmptyList()
        {
            // Arrange
            var input = new List<InformationInputDto>();

            // Act
            var result = _service.CalculateTotalPriceQuotes(input);

            // Assert
            Assert.Empty(result.Books);
        }

        


    }
}
