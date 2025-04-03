using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.UsersDTO
{
    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? Username { get; set; }

        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
    }
}
