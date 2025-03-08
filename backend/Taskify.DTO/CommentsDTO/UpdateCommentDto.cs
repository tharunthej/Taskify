using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.CommentsDTO
{
    public class UpdateCommentDto
    {
        [MaxLength(1000)]
        public string? Content { get; set; }
    }
}
