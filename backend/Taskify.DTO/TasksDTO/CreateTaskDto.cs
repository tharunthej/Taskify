using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.TasksDTO
{
    public class CreateTaskDto
    {
        [Required, MaxLength(150)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public required int StatusId { get; set; }

        [Required]
        public required int PriorityId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        // Optional: a task can be unassigned initially
        public int? AssignedTo { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }
}
