namespace Taskify.DTO.AttachmentsDTO
{
    public class AttachmentResponseDto
    {
        public int Id { get; set; }
        
        public int TaskId { get; set; }
        
        public string FileName { get; set; } = string.Empty;
        
        public string FileUrl { get; set; } = string.Empty;
        
        public int UploadedBy { get; set; }
        
        public DateTime UploadedAt { get; set; }
        
        // Flattened from the Uploader navigation property
        public string UploaderName { get; set; } = string.Empty;
    }
}
