using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignmentLunaEdge.DataAccess.Models
{
    public class User
    {
        public Guid ID {  get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow!;
    }
}
