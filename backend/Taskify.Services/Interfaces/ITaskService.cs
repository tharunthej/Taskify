using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        Task<TaskItem> GetTaskByIdAsync(int id);

        Task<TaskItem> CreateTaskAsync(TaskItem task);

        Task<TaskItem> UpdateTaskAsync(TaskItem task);
        
        Task DeleteTaskAsync(int id);
    }
}