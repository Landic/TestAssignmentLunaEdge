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
    // The TaskRepository class implements ITaskRepository interface
    public class TaskRepository : ITaskRepository
    {
        // AppDbContext instance to interact with the database
        private readonly AppDbContext _context;

        // Constructor that initializes the repository with the AppDbContext
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Method to get a task by its ID asynchronously
        public async Task<TaskModel> GetByIdAsync(Guid id) =>
            // Find and return the task with the given ID
            await _context.Tasks.FindAsync(id);

        // Method to get all tasks assigned to a user by their ID asynchronously
        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId) =>
            // Return a list of tasks where the UserID matches the provided userId
            await _context.Tasks.Where(t => t.UserID == userId).ToListAsync();

        // Method to add a new task to the database asynchronously
        public async Task AddAsync(TaskModel task)
        {
            // Add the task to the Tasks DbSet
            await _context.Tasks.AddAsync(task);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // Method to update an existing task in the database asynchronously
        public async Task UpdateAsync(TaskModel task)
        {
            // Update the task in the Tasks DbSet
            _context.Tasks.Update(task);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // Method to delete a task by its ID asynchronously
        public async Task DeleteAsync(Guid id)
        {
            // Get the task by its ID
            var task = await GetByIdAsync(id);
            // If the task exists, remove it from the database
            if (task != null)
            {
                _context.Tasks.Remove(task);
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
    }
}
