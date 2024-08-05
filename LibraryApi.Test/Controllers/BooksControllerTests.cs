using FluentAssertions;
using LibraryApi.Api.Controllers;
using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryApi.Test.Controllers
{
    public class BooksControllerTests
    {
        private readonly BooksController _controller;
        private readonly Mock<IBookService> _bookServiceMock;   

        public BooksControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BooksController(_bookServiceMock.Object);
        }

        [OneTimeTearDown]
        [Test, Fact]
        public async Task GetBooks_ReturnsOkResult_WithListOfBooksDto()
        {
            // Arrange
            var BookDTOs = new List<BookDTO>
            {
                new BookDTO { Id = new Guid(), Title = "Cien años de soledad", Author = "Gabriel García Márquez", Category = "Ficción", Description="", Copies=2 },
                new BookDTO { Id = new Guid(), Title = "El señor de los anillos", Author = "J.R.R. Tolkien", Category = "Fantasía", Description="", Copies=2 }
            };

            _bookServiceMock.Setup(s => s.GetBooksAsync())
                             .ReturnsAsync(BookDTOs);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<BookDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(BookDTOs);
        }

        [Test, Fact]
        public async Task CreateBook_ReturnsCreatedAtActionResult_WhenSuccessful()
        {
            // Arrange
            var bookDTO = new BookDTO
            {
                Id = new Guid(),
                Title = "Cien años de soledad",
                Author = "Gabriel García Márquez",
                Category = "Ficción",
                Description = "",
                Copies = 2
            };

            _bookServiceMock.Setup(s => s.CreateBookAsync(bookDTO))
                             .ReturnsAsync(bookDTO);

            // Act
            var result = await _controller.CreateBook(bookDTO);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be(201);
            var returnValue = createdAtActionResult.Value as BookDTO;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(bookDTO);
        }

        [Test, Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenSuccessful()
        {
            var id = new Guid();

            // Arrange
            _bookServiceMock.Setup(s => s.DeleteBookAsync(id))
                             .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteBook(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test, Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var id = new Guid();

            // Arrange
            _bookServiceMock.Setup(s => s.DeleteBookAsync(id))
                             .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteBook(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
