using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.ProjectsDTO
{
    public class CreateProjectDto
    {
        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
