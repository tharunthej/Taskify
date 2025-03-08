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

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] CreateUserDto createDto)
        {
            var newUser = _mapper.Map<User>(createDto);
            var createdUser = await _userService.CreateUserAsync(newUser);
            var response = _mapper.Map<UserResponseDto>(createdUser);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, response);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, user);
            await _userService.UpdateUserAsync(user);
            return NoContent();
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
