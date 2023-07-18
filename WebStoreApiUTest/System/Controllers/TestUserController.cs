using System.Threading.Tasks;
using WebStoreApi.Controllers;
using WebStoreApi.Services;
using WebStoreApiUTest.Mockdata;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace WebStoreApiUTest.System.Controllers
{
    public class TestUserController
    {
        [Fact]
        public async Task GetAsync_ReturnMustBe200()
        {
            var userService = new Mock<IUsersService>();

            userService.Setup(x => x.GetAsync())
                .ReturnsAsync(UserMockData.GetUsers());

            var sut = new UsersController(userService.Object);

            var result = (OkObjectResult)await sut.GetUsers();

            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAsync_ReturnMustBe204()
        {
            var userService = new Mock<IUsersService>();

            userService.Setup(x => x.GetAsync())
                .ReturnsAsync(UserMockData.GetUsers());

            var sut = new UsersController(userService.Object);

            var result = (NoContentResult)await sut.GetUsers();

            result.StatusCode.Should().Be(204);
            userService.Verify(x => x.GetAsync(), Times.Exactly(1));
        }
    }
}
