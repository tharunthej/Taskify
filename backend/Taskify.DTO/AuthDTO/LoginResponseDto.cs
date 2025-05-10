namespace Taskify.DTO.AuthDTO
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!; 
    }
}