namespace Taskify.DTO.TasksDTO
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public int StatusId { get; set; }

        public int PriorityId { get; set; }

        public int ProjectId { get; set; }
        
        // Flattened from the Project navigation property
        public required string ProjectName { get; set; }

        public int? AssignedTo { get; set; }
        
        // Flattened from the Assignee navigation property (if available)
        public string? AssigneeName { get; set; }

        public int CreatedBy { get; set; }
        
        // Flattened from the Creator navigation property
        public required string CreatorName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
