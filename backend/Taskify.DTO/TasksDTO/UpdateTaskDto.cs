using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.TasksDTO
{
    public class UpdateTaskDto
    {
        [MaxLength(150)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int? StatusId { get; set; }

        public int? PriorityId { get; set; }

        // Optionally update the assignee
        public int? AssignedTo { get; set; }
    }
}
