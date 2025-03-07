using Microsoft.EntityFrameworkCore;
using Taskify.Models.Models;
using TaskStatus = Taskify.Models.Models.TaskStatus;

namespace Taskify.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----- User Entity -----
            modelBuilder.Entity<User>(user =>
            {
                // User ↔ UserRole
                user.HasOne<UserRole>()
                    .WithMany()
                    .HasForeignKey(u => u.UserRoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User ↔ ProjectMember (ProjectMemberships)
                user.HasMany(u => u.ProjectMemberships)
                    .WithOne(pm => pm.User)
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // User ↔ TaskItem (AssignedTasks)
                user.HasMany(u => u.AssignedTasks)
                    .WithOne(t => t.Assignee)
                    .HasForeignKey(t => t.AssignedTo)
                    .OnDelete(DeleteBehavior.SetNull);

                // User ↔ Comment
                user.HasMany(u => u.Comments)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // User ↔ Notification
                user.HasMany(u => u.Notifications)
                    .WithOne(n => n.User)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- Attachment Entity -----
            modelBuilder.Entity<Attachment>(attachment =>
            {
                // Attachment ↔ TaskItem
                attachment.HasOne(a => a.Task)
                    .WithMany(t => t.Attachments)
                    .HasForeignKey(a => a.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Attachment ↔ Uploader (User)
                attachment.HasOne(a => a.Uploader)
                    .WithMany() // Optionally, add a navigation collection in User for Attachments.
                    .HasForeignKey(a => a.UploadedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ----- Comment Entity -----
            modelBuilder.Entity<Comment>(comment =>
            {
                // Comment ↔ TaskItem
                comment.HasOne(c => c.Task)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- Notification Entity -----
            modelBuilder.Entity<Notification>(notification =>
            {
                // Notification ↔ User
                notification.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- Project Entity -----
            modelBuilder.Entity<Project>(project =>
            {
                // Project ↔ Creator (User)
                project.HasOne(p => p.Creator)
                    .WithMany() // Optionally, add a CreatedProjects collection in User.
                    .HasForeignKey(p => p.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // Project ↔ ProjectMember (Members)
                project.HasMany(p => p.Members)
                    .WithOne(pm => pm.Project)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Project ↔ TaskItem (Tasks)
                project.HasMany(p => p.Tasks)
                    .WithOne(t => t.Project)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- ProjectMember Entity -----
            modelBuilder.Entity<ProjectMember>(projectMember =>
            {
                // ProjectMember ↔ Project
                projectMember.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ProjectMember ↔ User
                projectMember.HasOne(pm => pm.User)
                    .WithMany(u => u.ProjectMemberships)
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- TaskItem Entity -----
            modelBuilder.Entity<TaskItem>(task =>
            {
                // TaskItem ↔ Project
                task.HasOne(t => t.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(t => t.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                // TaskItem ↔ Creator (User)
                task.HasOne(t => t.Creator)
                    .WithMany() // Optionally, add a CreatedTasks collection in User.
                    .HasForeignKey(t => t.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                // TaskItem ↔ Assignee (User)
                task.HasOne(t => t.Assignee)
                    .WithMany(u => u.AssignedTasks)
                    .HasForeignKey(t => t.AssignedTo)
                    .OnDelete(DeleteBehavior.SetNull);

                // TaskItem ↔ TaskStatus (No navigation property)
                task.HasOne<TaskStatus>()
                    .WithMany()
                    .HasForeignKey(t => t.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                // TaskItem ↔ TaskPriority (No navigation property)
                task.HasOne<TaskPriority>()
                    .WithMany()
                    .HasForeignKey(t => t.PriorityId)
                    .OnDelete(DeleteBehavior.Restrict);

                // TaskItem ↔ Comment
                task.HasMany(t => t.Comments)
                    .WithOne(c => c.Task)
                    .HasForeignKey(c => c.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                // TaskItem ↔ Attachment
                task.HasMany(t => t.Attachments)
                    .WithOne(a => a.Task)
                    .HasForeignKey(a => a.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ----- Seed Data -----
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, Role = "Admin" },
                new UserRole { Id = 2, Role = "Member" },
                new UserRole { Id = 3, Role = "Guest" }
            );

            modelBuilder.Entity<TaskStatus>().HasData(
                new TaskStatus { Id = 1, Status = "To Do" },
                new TaskStatus { Id = 2, Status = "In Progress" },
                new TaskStatus { Id = 3, Status = "Done" }
            );

            modelBuilder.Entity<TaskPriority>().HasData(
                new TaskPriority { Id = 1, PriorityLevel = "Low" },
                new TaskPriority { Id = 2, PriorityLevel = "Medium" },
                new TaskPriority { Id = 3, PriorityLevel = "High" }
            );
        }

    }
}