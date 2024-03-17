using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using WebApiPerson.Context;
using WebApiPerson.Controllers;

namespace Test.WebPerson
{
    public class PersonControllerTests
    {
        [Fact]
        public async Task GetPersons_ReturnsListOfPersons()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options); 
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.GetPersons();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Person>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Person>>(actionResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetPerson_ReturnPerson()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.GetPerson(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Person>>(result);
            var model = Assert.IsAssignableFrom<Person>(actionResult.Value);
            Assert.Equal("John", model.Name);
        }

        [Fact]
        public async Task GetPerson_WithWrongID_ReturnNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.GetPerson(3);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutPerson_ReturnNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            var person = new Person { Id = 1, Name = "John", Age = 31 };
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.PutPerson(1, person);
            
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutPerson_WithWrongID_ReturnBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);
            var person = new Person { Id = 1, Name = "John", Age = 31 };
            
            // Act
            var result = await _controller.PutPerson(2, person);
            
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutPerson_WithWrongAge_ReturnBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);
            var person = new Person { Id = 1, Name = "John", Age = 19 };
            
            // Act
            var result = await _controller.PutPerson(1, person);
            
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostPerson_ReturnCreatedAtAction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            var _controller = new PersonController(dbContext);
            var person = new Person { Id = 3, Name = "Jack", Age = 35 };

            // Act
            var result = await _controller.PostPerson(person);

            //Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsAssignableFrom<Person>(actionResult.Value);
            Assert.Equal("Jack", model.Name);
        }

        [Fact]
        public async Task DeletePerson_ReturnNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.DeletePerson(1);
            
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePerson_WithWrongID_ReturnNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Persons.AddRange(new List<Person>
            {
                new Person { Id = 1, Name = "John", Age = 30 },
                new Person { Id = 2, Name = "Jane", Age = 25 }
            });
            await dbContext.SaveChangesAsync();
            var _controller = new PersonController(dbContext);

            // Act
            var result = await _controller.DeletePerson(3);
            
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
