using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<UserDTO> AuthenticateAsync(string usernameOrEmail, string password);
        Task RegisterAsync(UserDTO userDto, string password);
    }

}
