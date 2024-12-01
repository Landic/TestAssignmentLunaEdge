using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }
}
