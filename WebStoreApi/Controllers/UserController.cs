using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Update;
using WebStoreApi.Interfaces;
using MongoDB.Bson;

namespace WebStoreApi.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _usersService.Login(model);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usersService.GetAsync();

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var response = await _usersService.GetAsync(id);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpGet("details/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var response = await _usersService.GetByUsernameAsync(username);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterProfileRequest userDto)
        {
            await _usersService.CreateAsync(userDto);

            return Ok("User created successfully".ToJson());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateProfileRequest updateUser)
        {
            await _usersService.UpdateProfileAsync(id, updateUser);

            return Ok("User updated successfully.".ToJson());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _usersService.RemoveAsync(id);

            return Ok("User deleted successfully".ToJson());
        }
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(string id, UpdatePasswordRequest updatePassword)
        {
            await _usersService.UpdatePasswordAsync(id, updatePassword);
            return Ok("Password updated successfully".ToJson());
        }

        [HttpPost("{userId}/address")]
        public async Task<IActionResult> RegisterAddress(string userId, RegisterAddressRequest addressDto)
        {
            await _usersService.InsertAddress(userId, addressDto);
            return Ok("Address created successfully".ToJson());
        }

        [HttpPut("{userId}/address")]
        public async Task<IActionResult> UpdateAddress(string userId, UpdateAddressRequest updateAddress)
        {
            await _usersService.UpdateAddress(userId, updateAddress);
            return Ok("Address updated successfully".ToJson());
        }

        [HttpDelete("{userId}/address")]
        public async Task<IActionResult> DeleteAddress(string userId, string addressId)
        {
            await _usersService.DeleteAddress(userId, addressId);
            return Ok("Address deleted successfully".ToJson());
        }

        [HttpPost("{userId}/creditcard")]
        public async Task<IActionResult> RegisterCreditCard(string userId, RegisterCreditCardRequest creditCardDto)
        {
            await _usersService.InsertCreditCard(userId, creditCardDto);
            return Ok("Credit card created successfully".ToJson());
        }

        [HttpPut("{userId}/creditcard")]
        public async Task<IActionResult> UpdateCreditcard(string userId, string creditCardId, UpdateCreditCardRequest updateCreditCard)
        {
            await _usersService.UpdateCreditCard(userId, creditCardId, updateCreditCard);
            return Ok("Credit card updated successfully".ToJson());
        }

        [HttpDelete("{userId}/creditcard")]
        public async Task<IActionResult> DeleteCreditCard(string userId, string creditCardId)
        {
            await _usersService.DeleteCreditCard(userId, creditCardId);
            return Ok("Credit card deleted successfully".ToJson());
        }
    }
}
