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
    public class CategoriesControllerTests
    {
        private readonly CategoriesController _controller;
        private readonly Mock<ICategoryService> _categoryServiceMock;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        [Test, Fact]
        public async Task GetCategorias_ReturnsOkResult_WithListOfCategoriasDto()
        {
            // Arrange
            var categoriaDtos = new List<CategoryDTO>
            {
                new CategoryDTO { Id = new Guid(), Name = "Ficción", Description = "" },
                new CategoryDTO { Id = new Guid(), Name = "Fantasía", Description = "" }
            };

            _categoryServiceMock.Setup(s => s.GetCategoriesAsync())
                                 .ReturnsAsync(categoriaDtos);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<CategoryDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(categoriaDtos);
        }
    }
}
