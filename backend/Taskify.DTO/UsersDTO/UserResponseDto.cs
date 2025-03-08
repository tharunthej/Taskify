namespace Taskify.DTO.UsersDTO
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        
        public string Username { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public int UserRoleId { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
