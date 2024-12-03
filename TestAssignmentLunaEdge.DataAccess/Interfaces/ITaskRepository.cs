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
        Task<TaskModel> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userId);
        Task AddAsync(TaskModel task);
        Task UpdateAsync(TaskModel task);
        Task DeleteAsync(Guid id);
    }
}
