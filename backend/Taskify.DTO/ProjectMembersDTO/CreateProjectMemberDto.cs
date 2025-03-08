using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.ProjectMembersDTO
{
    public class CreateProjectMemberDto
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int UserRoleId { get; set; }
    }
}
