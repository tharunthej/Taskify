using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public required int StatusId { get; set; } // FK to TaskStatus

        [Required]
        public required int PriorityId { get; set; } // FK to TaskPriority

        public int ProjectId { get; set; }

        public int? AssignedTo { get; set; } // Nullable for unassigned tasks

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public required Project Project { get; set; }

        public User? Assignee { get; set; }

        public required User Creator { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Attachment>? Attachments { get; set; }
    }
}
