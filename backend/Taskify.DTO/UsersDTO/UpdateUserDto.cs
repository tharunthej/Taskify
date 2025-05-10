using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.UsersDTO
{
    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? Username { get; set; }

        [EmailAddress, MaxLength(100)]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}
