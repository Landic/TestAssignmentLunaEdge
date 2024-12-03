using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TestAssignmentLunaEdge.Controllers
{
    // The controller is responsible for handling task-related API requests
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensures that only authorized users can access the API methods
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Constructor that receives ITaskService to interact with task-related business logic
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Endpoint to get all tasks for the current user
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            // Extract user ID from JWT claims
            var userId = GetUserIdFromClaims();
            // Fetch the tasks associated with the user
            var tasks = await _taskService.GetUserTasksAsync(userId);
            return Ok(tasks); // Return the tasks in the response
        }

        // Endpoint to get a specific task by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            // Extract user ID from JWT claims
            var userId = GetUserIdFromClaims();
            // Fetch the task details by task ID and user ID
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            // If task is not found or user is unauthorized, return an error response
            if (task == null)
                return NotFound(new { message = "Task not found or access denied." });

            return Ok(task); // Return the task details in the response
        }

        // Endpoint to create a new task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDto)
        {
            // Validate the incoming model data
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // If model is invalid, return BadRequest

            // Extract user ID from JWT claims
            var userId = GetUserIdFromClaims();
            // Create the task
            await _taskService.CreateTaskAsync(taskDto, userId);
            return Ok(new { message = "Task created successfully." }); // Return success message
        }

        // Endpoint to update an existing task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDTO taskDto)
        {
            // Validate the incoming model data
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // If model is invalid, return BadRequest

            // Extract user ID from JWT claims
            var userId = GetUserIdFromClaims();
            taskDto.Id = id; // Set the task ID for updating

            try
            {
                // Try to update the task with the provided data
                await _taskService.UpdateTaskAsync(taskDto, userId);
                return Ok(new { message = "Task updated successfully." }); // Return success message
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // Return Forbidden if user is unauthorized
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Return BadRequest if any other error occurs
            }
        }

        // Endpoint to delete a task by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            // Extract user ID from JWT claims
            var userId = GetUserIdFromClaims();

            try
            {
                // Try to delete the task
                await _taskService.DeleteTaskAsync(id, userId);
                return Ok(new { message = "Task deleted successfully." }); // Return success message
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // Return Forbidden if user is unauthorized
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // Return BadRequest if any other error occurs
            }
        }

        // Private method to extract the user ID from the JWT claims
        private Guid GetUserIdFromClaims()
        {
            // Extract the user ID claim from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // If the user ID is not found or is invalid, throw an UnauthorizedAccessException
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID.");

            return userId; // Return the valid user ID
        }
    }
}