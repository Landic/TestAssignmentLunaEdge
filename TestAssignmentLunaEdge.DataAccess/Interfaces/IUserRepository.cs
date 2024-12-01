using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid Id);
    }
}
