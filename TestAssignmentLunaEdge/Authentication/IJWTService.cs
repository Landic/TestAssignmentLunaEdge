using System.Security.Claims;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Authentication
{
    public interface IJWTService
    {
        string GenerateJwtToken(User user);
        ClaimsPrincipal ValidateToken(string token);
    }
}
