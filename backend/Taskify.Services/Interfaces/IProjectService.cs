using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();

        Task<Project> GetProjectByIdAsync(int id);

        Task<Project> CreateProjectAsync(Project project);

        Task<Project> UpdateProjectAsync(Project project);

        Task DeleteProjectAsync(int id);
        
        Task AddMemberToProjectAsync(int projectId, int userId);
    }
}