using System.ComponentModel.DataAnnotations;

namespace Taskify.DTO.AttachmentsDTO
{
    public class CreateAttachmentDto
    {
        [Required]
        public int TaskId { get; set; }
        
        [Required, MaxLength(255)]
        public required string FileName { get; set; }
        
        [Required]
        public required string FileUrl { get; set; }
        
        [Required]
        public int UploadedBy { get; set; }
    }
}
