using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IEnumerable<ProjectMember>> GetAllProjectMembersAsync();

        Task<ProjectMember?> GetProjectMemberByIdAsync(int id);

        Task<ProjectMember> CreateProjectMemberAsync(ProjectMember projectMember);

        Task UpdateProjectMemberAsync(ProjectMember projectMember);

        Task DeleteProjectMemberAsync(int id);
    }
}
