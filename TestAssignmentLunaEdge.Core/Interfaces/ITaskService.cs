using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.Core.DTO;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDTO> GetTaskByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<TaskDTO>> GetUserTasksAsync(Guid userId);
        Task CreateTaskAsync(TaskDTO taskDto, Guid userId);
        Task UpdateTaskAsync(TaskDTO taskDto, Guid userId);
        Task DeleteTaskAsync(Guid taskId, Guid userId);
    }

}
