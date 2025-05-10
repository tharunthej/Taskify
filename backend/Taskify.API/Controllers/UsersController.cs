using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.UsersDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService, 
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return Ok(response);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<UserResponseDto>(user);
            return Ok(response);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();
    
                // Password update logic
                if (!string.IsNullOrEmpty(updateDto.NewPassword))
                {
                    if (string.IsNullOrEmpty(updateDto.CurrentPassword))
                    {
                        return BadRequest("Current password is required to set a new password");
                    }
    
                    if (!_userService.VerifyPassword(user, updateDto.CurrentPassword))
                    {
                        return BadRequest("Current password is incorrect");
                    }
    
                    user.PasswordHash = _userService.HashPassword(updateDto.NewPassword);
                }
    
                // Update other fields
                _mapper.Map(updateDto, user);
                
                // Clear password fields from the mapped user
                user.PasswordHash = user.PasswordHash; // Maintain the hash if not changing password
    
                await _userService.UpdateUserAsync(user);
    
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, "An error occurred while updating the user");
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
