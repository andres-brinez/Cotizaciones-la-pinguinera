using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.DesignPattern.Factories;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services;
using Sofka.Piguinera.Cotizacion.Services.Interface;

namespace Sofka.Piguinera.Cotizacion.Test.Services
{
    public class TotalPriceQuotationServiceTests
    {


        private readonly Mock<IBaseBookFactory> _factoryMock;
        private readonly Mock<DbSet<BookPersistence>> _dbSetMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly ITotalPriceQuotationService service;

        public TotalPriceQuotationServiceTests()
        {
            _dbSetMock = new Mock<DbSet<BookPersistence>>();
            _factoryMock = new Mock<IBaseBookFactory>();
            _databaseMock = new Mock<IDatabase>();
            _databaseMock.SetupGet(database => database.Books).Returns(_dbSetMock.Object);
            service = new TotalPriceQuotationService(_factoryMock.Object, _databaseMock.Object);

        }

        [Fact]
        public async void Create_ShouldReturnCorrectType_WhenCalledWithValidInput()
        {
            var employeeEntity = new BookEntity("6", "nombre de libro", 600, "Gustavo", 0, 6);
            var baseBookOutputDTO = new BaseBookOutputDTO("nombre de libro", BaseBookType.Book, 600, 0, 6);
            var employeeDTO = new BaseBookInputDTO("6", "nombre de libro", 600, "Gustavo", 600, 6, 0);


            _factoryMock
              .Setup(factory => factory.CreateBookEntity(It.IsAny<BaseBookInputDTO>()))
              .Returns(employeeEntity);

            _databaseMock
              .Setup(database => database.SaveAsync())
              .Returns(Task.FromResult(true));

            var result = await service.CalculateTotalPriceQuotation(employeeDTO);

            Assert.NotNull(result);
            Assert.IsType<BaseBookOutputDTO>(result);
            Assert.Equal(baseBookOutputDTO.Title, result.Title);
            Assert.Equal(baseBookOutputDTO.Discount, result.Discount);

            _databaseMock.Verify(database => database.Books.AddAsync(It.IsAny<BookPersistence>(), default), Times.Once);
            _databaseMock.Verify(database => database.SaveAsync(), Times.Once);
            _factoryMock.Verify(factory => factory.CreateBookEntity(It.IsAny<BaseBookInputDTO>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenDatabaseSaveFails()
        {
            var bookEntity = new BookEntity("6", "nombre de libro", 600, "Gustavo", 0, 6);
            var bookDTO = new BaseBookInputDTO("6", "nombre de libro", 600, "Gustavo", 600, 6, 0);

            _factoryMock
                .Setup(factory => factory.CreateBookEntity(It.IsAny<BaseBookInputDTO>()))
                .Returns(bookEntity);

            _databaseMock
               .Setup(database => database.SaveAsync())
               .Throws(new Exception("Database error"));

            var result = await service.CalculateTotalPriceQuotation(bookDTO);

            Assert.Null(result);
        }

        [Fact]
        public async Task CalculateTotalPriceQuotation_ShouldThrowException_WhenCalledWithNull()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => service.CalculateTotalPriceQuotation(null));
        }

    }

}
