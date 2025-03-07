using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class TaskPriority
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string PriorityLevel { get; set; } // Example: "Low", "Medium", "High"
    }
}
