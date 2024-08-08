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
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _userServiceMock;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        [Test, Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUserDto()
        {
            // Arrange
            var userDtos = new List<UserDTO>
            {
                new UserDTO { Id = new Guid(), Name = "Cruz, José Luis" },
                new UserDTO { Id = new Guid(), Name = "Lima, Angélica" }
            };

            _userServiceMock.Setup(s => s.GetUsersAsync())
                             .ReturnsAsync(userDtos);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnValue = okResult.Value as IEnumerable<UserDTO>;
            returnValue.Should().NotBeNull().And.BeEquivalentTo(userDtos);
        }
    }
}
