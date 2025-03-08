using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.CommentsDTO
{
    public class CreateCommentDto
    {
        [Required]
        public int TaskId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required, MaxLength(1000)]
        public required string Content { get; set; }
    }
}
