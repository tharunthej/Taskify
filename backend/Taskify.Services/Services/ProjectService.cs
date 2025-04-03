using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;
using Taskify.DTO.ProjectsDTO;
using AutoMapper;
using Taskify.Hubs;

namespace Taskify.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ProjectHub> _hubContext;
        private readonly IMapper _mapper;

        public ProjectService(
            AppDbContext context,
            IHubContext<ProjectHub> hubContext,
            IMapper mapper)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members!)
                .ThenInclude(m => m.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members!)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project ?? throw new KeyNotFoundException($"Project with ID {id} not found.");
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<ProjectResponseDto>(project);
                await _hubContext.Clients.All.SendAsync("ProjectCreated", responseDto);

                await transaction.CommitAsync();
                return project;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(project).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<ProjectResponseDto>(project);
                await _hubContext.Clients.All.SendAsync("ProjectUpdated", responseDto);

                await transaction.CommitAsync();
                return project;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project != null)
                {
                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("ProjectDeleted", id);
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task AddMemberToProjectAsync(int projectId, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == projectId);
                    
                var user = await _context.Users.FindAsync(userId);

                if (project == null || user == null)
                    throw new ArgumentException("Project or User not found.");

                var member = new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId,
                    UserRoleId = 2, // Member role
                    Project = project,
                    User = user
                };

                _context.ProjectMembers.Add(member);
                await _context.SaveChangesAsync();

                // Send to project-specific group
                await _hubContext.Clients.Group($"Project-{projectId}")
                    .SendAsync("MemberAdded", new { ProjectId = projectId, UserId = userId });

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}