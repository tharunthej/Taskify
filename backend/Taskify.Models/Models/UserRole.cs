using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Role { get; set; } // Example: "Admin", "Member", "Guest"
    }
}
