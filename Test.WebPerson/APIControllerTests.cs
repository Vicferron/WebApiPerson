using Xunit;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using SharedLibrary.Models;
using WebPerson.Client.Services;
using Test.WebPerson.Service;
using Microsoft.Extensions.Configuration;

namespace Test.WebPerson
{
    public class APIControllerTests
    {
        IConfiguration _config;

        public APIControllerTests()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["ApiSettings:BaseUrl"]).Returns("http://localhost:5000/");
            _config = mockConfig.Object;
        }

        [Fact]
        public async Task GetPersonsAsync_ReturnsListOfPersons()
        {
            //Arrange
            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            };
            var _httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK, persons));
            var apiController = new APIController(_httpClient, _config);

            // Act
            var result = await apiController.GetPersonsAsync();


            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Person>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].Name);
        }

        [Fact]
        public async Task GetPersonAsync_ReturnPerson()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "John", Age = 30 };

            var handler = new MockHttpMessageHandler(HttpStatusCode.OK, person);
            var httpClient = new HttpClient(handler);
            var apiController = new APIController(httpClient, _config);

            // Act
            var result = await apiController.GetPersonAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Person>(result);
            Assert.Equal("John", result.Name);
        }

        [Fact]
        public async Task GetPersonAsync_WithWrongID_ReturnHttpRequestException()
        {
            // Arrange
            var handler = new MockHttpMessageHandler(HttpStatusCode.NotFound);
            var httpClient = new HttpClient(handler);
            var apiController = new APIController(httpClient, _config);

            // Act and Assert
            var result = apiController.GetPersonAsync(1);
            await Assert.ThrowsAsync<HttpRequestException>(() => result);
        }

        [Fact]
        public async Task UpdatePersonAsync_WithUnsuccessfulHttpResponse_ThrowsException()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "John", Age = 30 };
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.BadRequest));
            var apiController = new APIController(httpClient, _config);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => apiController.UpdatePersonAsync(person));
        }

        [Fact]
        public async Task AddPersonAsync_WithUnsuccessfulHttpResponse_ThrowsException()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "John", Age = 30 };
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.BadRequest));
            var apiController = new APIController(httpClient, _config);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => apiController.AddPersonAsync(person));
        }

        [Fact]
        public async Task DeletePersonAsync_WithUnsuccessfulHttpResponse_ThrowsException()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "John", Age = 30 };
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.BadRequest));
            var apiController = new APIController(httpClient, _config);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => apiController.DeletePersonAsync(person.Id));
        }
    }
}