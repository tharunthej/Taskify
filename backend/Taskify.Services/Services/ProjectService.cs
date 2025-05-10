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

        public ProjectService(AppDbContext context, IHubContext<ProjectHub> hubContext, IMapper mapper)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int userId)
        {
            // Get all projects where the user is a member
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members!)
                    .ThenInclude(m => m.User)
                .Include(p => p.Tasks!)
                .Where(p => p.Members!.Any(m => m.UserId == userId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id, int userId)
        {
            var project = await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members!)
                    .ThenInclude(m => m.User)
                .Include(p => p.Tasks!)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) throw new KeyNotFoundException($"Project with ID {id} not found.");

            if (!HasProjectAccess(project, userId))
                throw new UnauthorizedAccessException("You don't have access to this project");

            return project;
        }
        
        public async Task<IEnumerable<Project>> GetAdminProjectsAsync(int userId)
        {
            return await _context.Projects
                .Include(p => p.Creator)
                .Include(p => p.Members!)
                    .ThenInclude(m => m.User)
                .Include(p => p.Tasks!)
                .Where(p => p.Members!.Any(m => 
                    m.UserId == userId && 
                    m.UserRoleId == 1
                ))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get creator from project.CreatedBy set by controller
                var creator = await _context.Users.FindAsync(project.CreatedBy);
                if (creator == null) throw new ArgumentException("Invalid user");

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                var adminMember = new ProjectMember
                {
                    ProjectId = project.Id,
                    UserId = project.CreatedBy,
                    UserRoleId = 1, // Admin
                    Project = project,
                    User = creator
                };

                _context.ProjectMembers.Add(adminMember);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ProjectCreated", _mapper.Map<ProjectResponseDto>(project));
                await transaction.CommitAsync();
                return project;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Project> UpdateProjectAsync(Project project, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingProject = await _context.Projects
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == project.Id);

                if (existingProject == null) 
                    throw new KeyNotFoundException("Project not found");

                // Add authorization check
                if (!IsProjectAdmin(existingProject, userId))
                    throw new UnauthorizedAccessException("Only project admins can update projects");

                // Update only allowed fields
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;

                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<ProjectResponseDto>(existingProject);
                await _hubContext.Clients.All.SendAsync("ProjectUpdated", responseDto);

                await transaction.CommitAsync();
                return existingProject;
            }
            catch  
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteProjectAsync(int id, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null) throw new KeyNotFoundException("Project not found");

                if (project.CreatedBy != userId)
                    throw new UnauthorizedAccessException("Only project creator can delete the project");

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ProjectDeleted", id);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task AddMemberToProjectAsync(int projectId, int userId, int currentUserId)
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
                    // Initialize required navigation properties
                    Project = project,
                    User = user
                };
        
                _context.ProjectMembers.Add(member);
                await _context.SaveChangesAsync();
        
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

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Members)
                        .ThenInclude(m => m.User)
                    .Include(p => p.Members)
                        .ThenInclude(m => m.Project)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                    throw new KeyNotFoundException($"Project with ID {projectId} not found.");

                if (!HasProjectAccess(project, userId))
                    throw new UnauthorizedAccessException("You don't have access to this project");

                await transaction.CommitAsync();
                return project.Members;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        #region Helpers
        private bool HasProjectAccess(Project project, int userId)
        {
            return project.Members!.Any(m => m.UserId == userId);
        }

        private bool IsProjectAdmin(Project project, int userId)
        {
            return project.Members!.Any(m =>    
                m.UserId == userId && m.UserRoleId == 1);
        }
        #endregion
    }
}