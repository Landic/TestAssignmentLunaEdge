using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskModel> CreateTaskAsync(Guid userid, string title, string? description, DateTime? dueDate);
        Task<IEnumerable<TaskModel>> GetTasksByUserIdAsync(Guid userid);
    }
}
