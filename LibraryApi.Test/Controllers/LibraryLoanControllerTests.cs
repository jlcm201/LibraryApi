using LibraryApi.Api.Controllers;
using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;


namespace LibraryApi.Test.Controllers
{
    public class LibraryLoanControllerTests
    {
        private readonly LibraryLoanController _controller;
        private readonly Mock<ILibraryLoanService> _libraryLoanServiceMock;

        public LibraryLoanControllerTests()
        {
            _libraryLoanServiceMock = new Mock<ILibraryLoanService>();
            _controller = new LibraryLoanController(_libraryLoanServiceMock.Object);
        }

        [Test, Fact]
        public async Task GetLibraryLoans_ReturnsOkResult_WithListOfLibraryLoanDto()
        {
            // Arrange
            var libraryLoansDto = new List<LibraryLoanDTO>
            {
                new LibraryLoanDTO
                {
                    Id = new Guid(),
                    BookTitle = "Cien años de soledad",
                    BookAuthor = "Gabriel García Márquez",
                    BookCategory = "Ficción",
                    User = "Cruz, José Luis",
                    LoanDate = DateTime.Now.AddDays(-10),
                    ReturnDate = null
                }
            };

            _libraryLoanServiceMock.Setup(s => s.GetLibraryLoanAsync())
                                .ReturnsAsync(libraryLoansDto);

            // Act
            var result = await _controller.GetLibraryLoans();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<LibraryLoanDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(libraryLoansDto);
        }

        [Test, Fact]
        public async Task GetLibraryLoansByUserId_ReturnsOkResult_WithListOfLibraryLoanDto()
        {
            // Arrange
            var libraryLoansDto = new List<LibraryLoanDTO>
            {
                new LibraryLoanDTO
                {
                    Id = new Guid(),
                    BookTitle = "Cien años de soledad",
                    BookAuthor = "Gabriel García Márquez",
                    BookCategory = "Ficción",
                    User = "Cruz, José Luis",
                    LoanDate = DateTime.Now.AddDays(-10),
                    ReturnDate = null
                }
            };

            var id = Guid.NewGuid();

            _libraryLoanServiceMock.Setup(s => s.GetLibraryLoansByUserId(id))
                                .ReturnsAsync(libraryLoansDto);

            // Act
            var result = await _controller.GetLibraryLoansByUserId(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<LibraryLoanDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(libraryLoansDto);
        }

        [Test, Fact]
        public async Task GetLibraryLoansByUserId_ReturnsNotFound_WhenLibraryLoanDoesNotExist()
        {
            var id = Guid.NewGuid();
            // Arrange
            _libraryLoanServiceMock.Setup(s => s.GetLibraryLoansByUserId(id)).ReturnsAsync(new List<LibraryLoanDTO>());

            // Act
            var result = await _controller.GetLibraryLoansByUserId(id);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test, Fact]
        public async Task CreateLibraryLoan_ReturnsCreatedAtActionResult_WhenSuccessful()
        {
            // Arrange
            var libraryLoansDto = new LibraryLoanDTO
            {
                Id = new Guid(),
                BookId = Guid.NewGuid(),
                BookTitle = "Cien años de soledad",
                BookAuthor = "Gabriel García Márquez",
                BookCategory = "Ficción",
                User = "Cruz, José Luis",
                LoanDate = DateTime.Now.AddDays(-10),
                ReturnDate = null
            };

            _libraryLoanServiceMock.Setup(s => s.GetBookAvailabilityAsync(libraryLoansDto.BookId))
                                .ReturnsAsync(true);
            _libraryLoanServiceMock.Setup(s => s.CreateLibraryLoanAsync(libraryLoansDto))
                                .ReturnsAsync(libraryLoansDto);

            // Act
            var result = await _controller.CreateLibraryLoan(libraryLoansDto);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be(201);
            var returnValue = createdAtActionResult.Value as LibraryLoanDTO;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(libraryLoansDto);
        }

        [Test, Fact]
        public async Task ReturnLibraryLoan_ReturnsNoContent_WhenSuccessful()
        {
            var id = Guid.NewGuid();
            // Arrange
            _libraryLoanServiceMock.Setup(s => s.DeleteLibraryLoanAsync(id))
                                .ReturnsAsync(true);

            // Act
            var result = await _controller.ReturnLibraryLoan(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test, Fact]
        public async Task ReturnLibraryLoan_ReturnsNotFound_WhenPrestamoDoesNotExist()
        {
            var id = Guid.NewGuid();
            // Arrange
            _libraryLoanServiceMock.Setup(s => s.DeleteLibraryLoanAsync(id))
                                .ReturnsAsync(false);

            // Act
            var result = await _controller.ReturnLibraryLoan(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
