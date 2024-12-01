using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(Guid Id)
        {
            return await _context.Users.FindAsync(Id);
        }
    }
}
