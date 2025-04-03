using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.AuthDTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, 
            ErrorMessage = "Password must be between 8 and 100 characters")]
        public required string Password { get; set; }
    }
}