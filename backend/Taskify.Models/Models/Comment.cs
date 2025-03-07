using System;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        
        public int UserId { get; set; }

        [Required, MaxLength(1000)]
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public required TaskItem Task { get; set; }
        
        public required User User { get; set; }
    }
}
