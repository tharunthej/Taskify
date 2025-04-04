using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.AuthDTO;
using Taskify.DTO.UsersDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;
using Taskify.Services.Exceptions;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IMapper mapper,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _authService.AuthenticateUser(loginDto.Email, loginDto.Password);
                var token = _authService.GenerateJwtToken(user);
                return Ok(_mapper.Map<LoginResponseDto>(user, opt => opt.Items["Token"] = token));
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogWarning(ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var user = _mapper.Map<User>(createUserDto);
                var createdUser = await _authService.RegisterUser(user, createUserDto.Password);
                var token = _authService.GenerateJwtToken(createdUser);
                return CreatedAtAction(nameof(Login), 
                    _mapper.Map<LoginResponseDto>(createdUser, opt => opt.Items["Token"] = token));
            }
            catch (EmailAlreadyExistsException ex)
            {
                _logger.LogWarning(ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}