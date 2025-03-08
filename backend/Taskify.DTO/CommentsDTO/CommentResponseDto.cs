namespace Taskify.DTO.CommentsDTO
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        
        public int TaskId { get; set; }
        
        public int UserId { get; set; }
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        
        // Flattened from the Comment.User navigation property.
        public string UserName { get; set; } = string.Empty;
    }
}
