using System;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(500)]
        public required string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public required User User { get; set; }
    }
}
