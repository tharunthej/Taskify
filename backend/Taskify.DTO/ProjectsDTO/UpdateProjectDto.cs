using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.ProjectsDTO
{
    public class UpdateProjectDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}