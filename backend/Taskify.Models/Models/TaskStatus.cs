using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class TaskStatus
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Status { get; set; } // Example: "To Do", "In Progress", "Done"
    }
}
