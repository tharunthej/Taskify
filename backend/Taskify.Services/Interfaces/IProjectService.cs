using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync(int userId);

        Task<Project> GetProjectByIdAsync(int id, int userId);

        Task<IEnumerable<Project>> GetAdminProjectsAsync(int userId);

        Task<Project> CreateProjectAsync(Project project);
        
        Task<Project> UpdateProjectAsync(Project project, int userId);

        Task DeleteProjectAsync(int id, int userId);
        
        Task AddMemberToProjectAsync(int projectId, int userId, int currentUserId);

        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(int projectId, int userId);
    }
}