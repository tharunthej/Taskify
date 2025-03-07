using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public required User Creator { get; set; }

        public ICollection<ProjectMember>? Members { get; set; }
        
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
