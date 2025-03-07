using System;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        [Required, MaxLength(255)]
        public required string FileName { get; set; }

        [Required]
        public required string FileUrl { get; set; }

        public int UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public required TaskItem Task { get; set; }
        
        public required User Uploader { get; set; }
    }
}
