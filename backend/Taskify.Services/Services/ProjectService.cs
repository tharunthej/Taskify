using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddMemberToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
                throw new ArgumentException("Project or User not found.");

            var member = new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId,
                UserRoleId = 2, // Default to "Member" role
                Project = project,
                User = user
            };

            _context.ProjectMembers.Add(member);
            await _context.SaveChangesAsync();
        }
    }
}