using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Collections;
using WebStoreApi.Services;
using MongoDB.Bson;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using WebStoreApi.Collections.ViewModels.Users.Authorization;
using System.Security.Claims;
using WebStoreApi.Collections.ViewModels.Users.Register;
using WebStoreApi.Collections.ViewModels.Users.Update;

namespace WebStoreApi.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = _usersService.Authenticate(model);
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
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID.");
            }

            var response = await _usersService.GetAsync(objectId);
            if (response == null) return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterProfileRequest newUser )
        {
            await _usersService.CreateAsync(newUser);
            return CreatedAtAction(nameof(GetUser), new {Username = newUser.Username}, newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateProfileRequest updateUser)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID.");
            }
            
            await _usersService.UpdateProfileAsync(objectId, updateUser);
            return Ok("User updated successfully.");   
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID.");
            }

            await _usersService.RemoveAsync(objectId);
            return NoContent();
        }
    }
}
