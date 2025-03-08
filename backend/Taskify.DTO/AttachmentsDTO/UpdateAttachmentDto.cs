using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.AttachmentsDTO
{
    public class UpdateAttachmentDto
    {
        [MaxLength(255)]
        public string? FileName { get; set; }
        
        public string? FileUrl { get; set; }
    }
}
