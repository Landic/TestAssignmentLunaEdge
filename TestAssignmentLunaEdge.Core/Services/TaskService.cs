using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskModel> CreateTaskAsync(Guid userid, string title, string? description, DateTime? dueDate)
        {
            var task = new TaskModel 
            {
                UserID = userid,
                Title = title,
                Description = description,
                DueDate = dueDate,
            };
            return await _taskRepository.AddTaskAsync(task);
        }

        public async Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userid)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userid);
        }
    }
}
