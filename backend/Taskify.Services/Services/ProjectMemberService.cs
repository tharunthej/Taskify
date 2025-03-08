using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.Services.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly AppDbContext _context;

        public ProjectMemberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectMember>> GetAllProjectMembersAsync()
        {
            return await _context.ProjectMembers
                .Include(pm => pm.Project)
                .Include(pm => pm.User)
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetProjectMemberByIdAsync(int id)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.Project)
                .Include(pm => pm.User)
                .FirstOrDefaultAsync(pm => pm.Id == id);
        }

        public async Task<ProjectMember> CreateProjectMemberAsync(ProjectMember projectMember)
        {
            _context.ProjectMembers.Add(projectMember);
            await _context.SaveChangesAsync();
            return projectMember;
        }

        public async Task UpdateProjectMemberAsync(ProjectMember projectMember)
        {
            _context.Entry(projectMember).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectMemberAsync(int id)
        {
            var projectMember = await _context.ProjectMembers.FindAsync(id);
            if (projectMember != null)
            {
                _context.ProjectMembers.Remove(projectMember);
                await _context.SaveChangesAsync();
            }
        }
    }
}
