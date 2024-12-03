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
    // The UserRepository class implements IUserRepository interface
    public class UserRepository : IUserRepository
    {
        // AppDbContext instance to interact with the database
        private readonly AppDbContext _context;

        // Constructor that initializes the repository with the AppDbContext
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Method to get a user by their ID asynchronously
        public async Task<User> GetByIdAsync(Guid id)
        {
            // Find and return the user with the given ID
            return await _context.Users.FindAsync(id);
        }

        // Method to get a user by their username or email asynchronously
        public async Task<User> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            // Return the first user found where the username or email matches the input
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        // Method to add a new user to the database asynchronously
        public async Task AddAsync(User user)
        {
            // Add the user to the Users DbSet
            await _context.Users.AddAsync(user);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
