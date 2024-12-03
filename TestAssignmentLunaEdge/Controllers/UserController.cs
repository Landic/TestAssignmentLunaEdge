using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TestAssignmentLunaEdge.Authentication;
using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.Core.Services;
using TestAssignmentLunaEdge.DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestAssignmentLunaEdge.Controllers
{
    // API Controller for user-related actions (registration, login)
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; // Service for user-related business logic
        private readonly IJWTService _jwtService;   // Service for handling JWT generation

        // Constructor to initialize services for user handling and JWT management
        public UserController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        // Endpoint to register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            // Validate the incoming request model
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return a BadRequest if the model is invalid

            // Register the new user by passing the provided DTO and password
            await _userService.RegisterAsync(
                new UserDTO
                {
                    Username = registerUserDto.Username,
                    Email = registerUserDto.Email
                },
                registerUserDto.Password
            );

            return Ok(new { message = "User registered successfully." }); // Return success message
        }

        // Endpoint to login an existing user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            // Validate the incoming request model
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return a BadRequest if the model is invalid

            // Authenticate the user by checking the provided credentials
            var user = await _userService.AuthenticateAsync(loginUserDto.UsernameOrEmail, loginUserDto.Password);

            // If authentication fails, return Unauthorized with a message
            if (user == null)
                return Unauthorized(new { message = "Invalid username, email, or password." });

            // If authentication succeeds, generate a JWT token for the user
            var userEntity = new User
            {
                ID = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            var token = _jwtService.GenerateJwtToken(userEntity); // Generate the JWT token

            // Return the generated token in the response
            return Ok(new { token });
        }
    }
}
