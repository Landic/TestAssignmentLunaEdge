using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignmentLunaEdge.DataAccess.Models
{
    public class TaskModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow!;

        public Guid UserID { get; set; }
        public User User { get; set; } = null!;

    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}
