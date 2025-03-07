using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public required int UserRoleId { get; set; } // FK to UserRole

        // Navigation Properties
        public required Project Project { get; set; }
        
        public required User User { get; set; }
    }
}
