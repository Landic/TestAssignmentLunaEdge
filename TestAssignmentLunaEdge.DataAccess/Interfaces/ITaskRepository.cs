using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskModel?> AddTaskAsync(TaskModel task);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId);
        Task<TaskModel?> GetTaskByIdAsync(Guid id);
        Task UpdateTaskAsync(TaskModel task);
        Task DeleteTaskAsync(Guid id);
    }
}
