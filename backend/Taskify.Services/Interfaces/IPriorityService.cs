using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IPriorityService
    {
        Task<IEnumerable<TaskPriority>> GetAllPrioritiesAsync();
    }
}