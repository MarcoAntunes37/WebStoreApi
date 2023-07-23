using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using Xunit;
using WebStoreApi.Controllers;
using WebStoreApi.Services;
using WebStoreApi.Collections;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;

namespace WebStoreApiTeste
{
    public class UserControllerTest
    {
        private readonly UsersController _usersController;
        private readonly Mock<IUsersService> _userServiceMock = new Mock<IUsersService>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public UserControllerTest()
        {
            _usersController = new UsersController(_userServiceMock.Object);
        }
        #region getUsers
        [Fact]
        public async Task GetUsersAsync_ShouldReturnArrayOfUsers_WhenUsersExist()
        {
            List<User> users = new List<User>(){
                new User{
                    Id = ObjectId.GenerateNewId().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    Telephone = "+1 123-456-7890",
                    Username = "johndoe",
                    Password = "password123",
                    Email = "johndoe@example.com"
                },
                new User{
                    Id = ObjectId.GenerateNewId().ToString(),
                    FirstName = "John1",
                    LastName = "Doe",
                    Telephone = "+1 123-456-7890",
                    Username = "johndoe",
                    Password = "password123",
                    Email = "johndoe@example.com",
                },
                new User{
                    Id = ObjectId.GenerateNewId().ToString(),
                    FirstName = "John2",
                    LastName = "Doe",
                    Telephone = "+1 123-456-7890",
                    Username = "johndoe",
                    Password = "password123",
                    Email = "johndoe@example.com",
                },
            };

            _userServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(users);

            var result = await _usersController.GetUsers();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnNotFound_WhenUserNotExist()
        {
            List<User> users = null;

            _userServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(users);

            var result = await _usersController.GetUsers();

            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region getUserById
        [Fact]
        public async Task GetUserAsync_ShouldReturnAUser_WhenUserExist()
        {
            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FirstName = "John",
                LastName = "Doe",
                Telephone = "+1 123-456-7890",
                Username = "johndoe",
                Password = "password123",
                Email = "johndoe@example.com"
            };

            _userServiceMock.Setup(x => x.GetAsync(user.Id)).ReturnsAsync(user);

            var result = await _usersController.GetUser(user.Id);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnNotFound_WhenUserNotExist()
        {

            var userId = ObjectId.GenerateNewId().ToString();

            _userServiceMock.Setup(x => x.GetAsync(userId)).ReturnsAsync(() => null);

            var result = await _usersController.GetUser(userId);

            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region updateUser
        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenUserUpdated()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var user = new UpdateProfileRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Telephone = "+1 123-456-7890",
                Password = "password123",
                Email = "johndoe@example.com"
            };

            _userServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(new User { Id = routeId });

            _userServiceMock.Setup(x => x.UpdateProfileAsync(routeId, user));

            var result = await _usersController.UpdateUser(routeId, user);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserNotExist()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var user = new UpdateProfileRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Telephone = "+1 123-456-7890",
                Password = "password123",
                Email = "johndoe@example.com"
            };

            _userServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _usersController.UpdateUser(routeId, user);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserIdIsDifferentToRouteId()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            var user = new UpdateProfileRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Telephone = "+1 123-456-7890",
                Password = "password123",
                Email = "johndoe@example.com"
            };

            _userServiceMock.Setup(x => x.GetAsync(routeId)).ReturnsAsync(() => null);

            var result = await _usersController.UpdateUser(routeId, user);

            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region Delete User
        [Fact]
        public async Task DeleteUser_ShouldReturnBadRequest_WhenUserNotExists()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _userServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _usersController.DeleteUser(routeId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserIsDeleted()
        {
            var routeId = ObjectId.GenerateNewId().ToString();

            _userServiceMock.Setup(x => x.RemoveAsync(routeId));

            var result = await _usersController.DeleteUser(routeId);

            Assert.IsType<OkObjectResult>(result);
        }
        #endregion
    }
}
