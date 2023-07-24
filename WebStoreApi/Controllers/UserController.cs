using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Services;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Update;
using Microsoft.AspNetCore.Authorization;
using WebStoreApi.Interfaces;

namespace WebStoreApi.Controllers
{
    
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _usersService.Login(model);

            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost("logout/{id}")]
        public async Task<IActionResult> Logout(string id)
        {
            await _usersService.Logout(id);

            return Ok("User logout successfully");
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

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterProfileRequest userDto )
        {
            await _usersService.CreateAsync(userDto);

            return Ok("User created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateProfileRequest updateUser)
        {            
            await _usersService.UpdateProfileAsync(id, updateUser);

            return Ok("User updated successfully.");   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _usersService.RemoveAsync(id);

            return Ok("User deleted successfully");
        }
    }
}
