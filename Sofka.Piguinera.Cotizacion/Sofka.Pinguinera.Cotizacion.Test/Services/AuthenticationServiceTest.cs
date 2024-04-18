using Microsoft.EntityFrameworkCore;
using Moq;
using Sofka.Piguinera.Cotizacion.Database;
using Sofka.Piguinera.Cotizacion.Database.Configuration.Interfaces;
using Sofka.Piguinera.Cotizacion.Models.DTOS.Input;
using Sofka.Piguinera.Cotizacion.Models.Persistence;
using Sofka.Piguinera.Cotizacion.Services.Implementations;
using Sofka.Piguinera.Cotizacion.Services.Interface;


namespace Sofka.Pinguinera.Cotizacion.Test.Services
{
    public class AuthenticationServiceTest
    {

        private readonly Mock<IDataBaseService> _databaseMock; 
        private readonly IAuthenticationService AuthenticationService;

        public AuthenticationServiceTest()
        {
            _databaseMock = new Mock<IDataBaseService>();
            AuthenticationService = new AuthenticationService(_databaseMock.Object);
        }

       
        [Fact]
        public async Task RegisterUser_ShouldReturnTrue_WhenUserIsSaved()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            mockDatabaseService.Setup(db => db.AddUserAsync(It.IsAny<UserPersistence>())).ReturnsAsync(true);

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var user = new RegisterUserInputDTO("test@test.com", "password", "username");
            var result = await authService.RegisterUser(user);
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFalse_WhenInputDataIsBlank()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            mockDatabaseService.Setup(db => db.AddUserAsync(It.IsAny<UserPersistence>())).ReturnsAsync(true);

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var user = new RegisterUserInputDTO("", "", "");
            var result = await authService.RegisterUser(user);
            Assert.False(result);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            mockDatabaseService.Setup(db => db.GetUser(It.IsAny<string>())).Returns(new UserPersistence("test@test.com", BCrypt.Net.BCrypt.HashPassword("password"), "username"));

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var user = new LoginUserInputDTO("test@test.com", "password");
            var result = await authService.LoginUser(user);
            Assert.True(result);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnFalse_WhenCredentialsAreIncorrect()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            mockDatabaseService.Setup(db => db.GetUser(It.IsAny<string>())).Returns(new UserPersistence("correct@test.com", BCrypt.Net.BCrypt.HashPassword("correctPassword"), "correctUsername"));

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var user = new LoginUserInputDTO("incorrect@test.com", "incorrectPassword");
            var result = await authService.LoginUser(user);
            Assert.False(result);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            var expectedUser = new UserPersistence("correct@test.com", BCrypt.Net.BCrypt.HashPassword("correctPassword"), "correctUsername");
            mockDatabaseService.Setup(db => db.GetUser("correct@test.com")).Returns(expectedUser);

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var result =  authService.GetUser("correct@test.com");
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            var mockDatabaseService = new Mock<IDataBaseService>();
            mockDatabaseService.Setup(db => db.GetUser(It.IsAny<string>())).Returns((UserPersistence)null);

            var authService = new AuthenticationService(mockDatabaseService.Object);
            var result =  authService.GetUser("incorrect@test.com");
            Assert.Null(result);
        }


    }
}
