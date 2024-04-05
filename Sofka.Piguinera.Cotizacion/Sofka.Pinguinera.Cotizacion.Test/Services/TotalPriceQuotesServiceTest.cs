using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.DTOS.InputDTO;
using Sofka.Piguinera.Cotizacion.Models.DTOS.OutputDTO;
using Sofka.Piguinera.Cotizacion.Models.Entities;
using Sofka.Piguinera.Cotizacion.Models.Enums;
using Sofka.Piguinera.Cotizacion.Models.Factories;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services;
using Sofka.Piguinera.Cotizacion.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class TotalPriceQuotesServiceTest
    {

        private readonly Mock<DbSet<BookPersistence>> _dbSetMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly ITotalPriceQuotesService service;

        public TotalPriceQuotesServiceTest()
        {
            _dbSetMock = new Mock<DbSet<BookPersistence>>();
            _databaseMock = new Mock<IDatabase>();
            _databaseMock.SetupGet(database => database.Books).Returns(_dbSetMock.Object);
            service = new TotalPriceQuotesService(_databaseMock.Object);
        }

        public async void Create_ShouldReturnCorrectType_WhenCalledWithValidInput()
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
        public async Task CalculateTotalPriceQuotes_ShouldReturnNull_WhenDatabaseSaveFails()
        {
            var informationInputDtoList = new List<InformationInputDto>
            {
                new InformationInputDto("1", 5),
                new InformationInputDto("2", 10)
            };

            _databaseMock
                .Setup(database => database.SaveAsync())
                .ReturnsAsync(false); // Simulate a failure when trying to save changes

            var result = service.CalculateTotalPriceQuotes(informationInputDtoList);

            Assert.Null(result);
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
