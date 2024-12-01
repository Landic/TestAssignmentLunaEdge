using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<User> RegisterUserAsync(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = password
            };
            return await _userRepository.AddUserAsync(user);
        }
    }
}
