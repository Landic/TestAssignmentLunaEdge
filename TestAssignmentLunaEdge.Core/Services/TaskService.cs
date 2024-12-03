using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.Core;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Services
{
    using Microsoft.Extensions.Logging;

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        // Constructor to inject dependencies (ITaskRepository and ILogger)
        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        // Method to fetch a task by its ID asynchronously, and ensure the user is authorized to view it
        public async Task<TaskDTO> GetTaskByIdAsync(Guid id, Guid userId)
        {
            _logger.LogInformation($"Fetching task with ID: {id} for user ID: {userId}");

            // Retrieve the task from the repository
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null || task.UserID != userId)
            {
                // Log warning if task is not found or user is not authorized
                _logger.LogWarning($"Task with ID: {id} not found or user is not authorized.");
                return null;
            }

            // Log success and return the task data in a DTO (Data Transfer Object)
            _logger.LogInformation($"Task with ID: {id} found for user ID: {userId}");
            return new TaskDTO
            {
                Id = task.ID,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString()
            };
        }

        // Method to fetch all tasks for a specific user asynchronously
        public async Task<IEnumerable<TaskDTO>> GetUserTasksAsync(Guid userId)
        {
            _logger.LogInformation($"Fetching tasks for user ID: {userId}");

            // Retrieve all tasks for the user
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            _logger.LogInformation($"Found {tasks.Count()} tasks for user ID: {userId}");

            // Map each task to a DTO and return as a collection
            return tasks.Select(t => new TaskDTO
            {
                Id = t.ID,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString()
            });
        }

        // Method to create a new task for a user asynchronously
        public async Task CreateTaskAsync(TaskDTO taskDto, Guid userId)
        {
            _logger.LogInformation($"Creating task for user ID: {userId} with title: {taskDto.Title}");

            // Create a new TaskModel object
            var task = new TaskModel
            {
                ID = Guid.NewGuid(),
                UserID = userId, // Assign the user ID
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = Enum.Parse<DataAccess.Models.TaskStatus>(taskDto.Status), // Parse status from DTO
                Priority = Enum.Parse<TaskPriority>(taskDto.Priority), // Parse priority from DTO
                CreatedAt = DateTime.UtcNow, // Set the creation time
                UpdatedAt = DateTime.UtcNow // Set initial update time
            };

            // Save the new task to the repository
            await _taskRepository.AddAsync(task);
            _logger.LogInformation($"Task with title: {taskDto.Title} created successfully for user ID: {userId}");
        }

        // Method to update an existing task asynchronously
        public async Task UpdateTaskAsync(TaskDTO taskDto, Guid userId)
        {
            _logger.LogInformation($"Updating task with ID: {taskDto.Id} for user ID: {userId}");

            // Retrieve the task by its ID
            var task = await _taskRepository.GetByIdAsync(taskDto.Id);
            if (task == null || task.UserID != userId)
            {
                // Log warning and throw exception if user is not authorized to update the task
                _logger.LogWarning($"User ID: {userId} is not authorized to update task with ID: {taskDto.Id}");
                throw new UnauthorizedAccessException("Not authorized to update this task.");
            }

            // Update the task fields based on the DTO
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Status = Enum.Parse<DataAccess.Models.TaskStatus>(taskDto.Status);
            task.Priority = Enum.Parse<TaskPriority>(taskDto.Priority);
            task.UpdatedAt = DateTime.UtcNow; // Update the time of modification

            // Save the updated task to the repository
            await _taskRepository.UpdateAsync(task);
            _logger.LogInformation($"Task with ID: {taskDto.Id} updated successfully for user ID: {userId}");
        }

        // Method to delete a task asynchronously
        public async Task DeleteTaskAsync(Guid taskId, Guid userId)
        {
            _logger.LogInformation($"Deleting task with ID: {taskId} for user ID: {userId}");

            // Retrieve the task by its ID
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null || task.UserID != userId)
            {
                // Log warning and throw exception if user is not authorized to delete the task
                _logger.LogWarning($"User ID: {userId} is not authorized to delete task with ID: {taskId}");
                throw new UnauthorizedAccessException("Not authorized to delete this task.");
            }

            // Delete the task from the repository
            await _taskRepository.DeleteAsync(taskId);
            _logger.LogInformation($"Task with ID: {taskId} deleted successfully for user ID: {userId}");
        }
    }
}
