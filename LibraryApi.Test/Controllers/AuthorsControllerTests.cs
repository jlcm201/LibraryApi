using FluentAssertions;
using LibraryApi.Api.Controllers;
using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApi.Test.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly AuthorsController _controller;
        private readonly Mock<IAuthorService> _authorServiceMock;

        public AuthorsControllerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _controller = new AuthorsController(_authorServiceMock.Object);
        }

        [Test, Fact]
        public async Task GetAutores_ReturnsOkResult_WithListOfAutoresDto()
        {
            // Arrange
            var authorDtos = new List<AuthorDTO>
            {
                new AuthorDTO { Id = new Guid(), Name = "Gabriel García Márquez", Description = "" },
                new AuthorDTO { Id = new Guid(), Name = "J.R.R. Tolkien", Description = "" }
            };

            _authorServiceMock.Setup(s => s.GetAuthorsAsync())
                             .ReturnsAsync(authorDtos);

            // Act
            var result = await _controller.GetAuthors();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<AuthorDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(authorDtos);
        }

    }
}
