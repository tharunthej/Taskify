using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.UsersDTO
{
    public class CreateUserDto
    {
        [Required, MaxLength(50)]
        public required string Username { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
