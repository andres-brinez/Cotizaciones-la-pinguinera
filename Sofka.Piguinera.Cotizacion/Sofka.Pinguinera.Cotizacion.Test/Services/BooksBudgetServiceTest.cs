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
    public class BooksBudgetServiceTest
    {

        private readonly Mock<IBaseBookFactory> _factoryMock;
        private readonly Mock<DbSet<BookPersistence>> _dbSetMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly IBooksBudgetService service;

        public BooksBudgetServiceTest()
        {
            _dbSetMock = new Mock<DbSet<BookPersistence>>();
            _databaseMock = new Mock<IDatabase>();
            _databaseMock.SetupGet(database => database.Books).Returns(_dbSetMock.Object);
            service = new BooksBudgetService(_databaseMock.Object);
        }


        [Fact]
        public async void Create_ShouldReturnCorrectType_WhenCalledWithValidInput()
        {
            BookWithBudgeInputDTO books = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };


    

            _databaseMock
              .Setup(database => database.SaveAsync())
              .Returns(Task.FromResult(true));

            var result = service.BooksBudget(books);

            Assert.NotNull(result);
            Assert.IsType<BaseBookOutputDTO>(result);
            // comparar que el Budget sea nayor a 0


            _databaseMock.Verify(database => database.Books.AddAsync(It.IsAny<BookPersistence>(), default), Times.Once);
            _databaseMock.Verify(database => database.SaveAsync(), Times.Once);
            _factoryMock.Verify(factory => factory.Create(It.IsAny<BaseBookInputDTO>()), Times.Once);
        }

        [Fact]
        public async Task CalculateTotalPriceQuotes_ShouldReturnNull_WhenDatabaseSaveFails()
        {
            var informationBooks = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> { "1", "2" },
                Budget = 1000
            };

            _databaseMock
                .Setup(database => database.SaveAsync())
                .ReturnsAsync(false); // Simulate a failure when trying to save changes

            var result = service.BooksBudget(informationBooks);

            Assert.Null(result);
        }

        [Fact]
        public async Task CalculateTotalPriceQuotes_ShouldHandleEmptyList()
        {
            var informationBooks = new BookWithBudgeInputDTO
            {
                IdBooks = new List<string> {},
                Budget = 
            };

            var result = service.BooksBudget(informationBooks);

            Assert.Empty(result.Books);

        }



    }
}
