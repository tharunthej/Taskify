using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taskify.Models.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Username { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required int UserRoleId { get; set; } // FK to UserRole

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        

        // Navigation Properties
        public ICollection<ProjectMember>? ProjectMemberships { get; set; }

        public ICollection<TaskItem>? AssignedTasks { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
