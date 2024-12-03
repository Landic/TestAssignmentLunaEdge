using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Services
{
    using Microsoft.Extensions.Logging;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        // Constructor to inject dependencies (IUserRepository and ILogger)
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // Method to fetch a user by their ID asynchronously
        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching user with ID: {id}");

            // Retrieve the user from the repository
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                // Log warning if user not found
                _logger.LogWarning($"User with ID: {id} not found");
                return null;
            }

            // Log success and return the user data in a DTO (Data Transfer Object)
            _logger.LogInformation($"User with ID: {id} found");
            return new UserDTO { Id = user.ID, Username = user.Username, Email = user.Email };
        }

        // Method to authenticate a user based on username/email and password asynchronously
        public async Task<UserDTO> AuthenticateAsync(string usernameOrEmail, string password)
        {
            _logger.LogInformation($"Attempting authentication for user: {usernameOrEmail}");

            // Fetch user by username or email
            var user = await _userRepository.GetByUsernameOrEmailAsync(usernameOrEmail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                // Log warning if authentication fails
                _logger.LogWarning($"Failed authentication attempt for user: {usernameOrEmail}");
                return null;
            }

            // Log success and return user data in a DTO
            _logger.LogInformation($"Authentication successful for user: {usernameOrEmail}");
            return new UserDTO { Id = user.ID, Username = user.Username, Email = user.Email };
        }

        // Method to register a new user asynchronously
        public async Task RegisterAsync(UserDTO userDto, string password)
        {
            _logger.LogInformation($"Registering new user with username: {userDto.Username}");

            // Create a new User entity, hashing the password before saving it
            var user = new User
            {
                ID = Guid.NewGuid(), // Generate a new unique ID
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password before storing
                CreatedAt = DateTime.UtcNow, // Set creation time
                UpdatedAt = DateTime.UtcNow // Set initial update time
            };

            // Save the new user in the repository
            await _userRepository.AddAsync(user);
            _logger.LogInformation($"User {userDto.Username} registered successfully");
        }
    }
}
