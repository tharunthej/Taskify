namespace Taskify.DTO.ProjectsDTO
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string CreatorName { get; set; } // Flattened from User
    }
}