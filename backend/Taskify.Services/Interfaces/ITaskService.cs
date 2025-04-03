using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId);

        Task<TaskItem> GetTaskByIdAsync(int id, int userId);

        Task<TaskItem> CreateTaskAsync(TaskItem task, int userId);

        Task<TaskItem> UpdateTaskAsync(TaskItem task, int userId);
        
        Task DeleteTaskAsync(int id, int userId);
    }
}