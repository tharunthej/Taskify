using TaskStatus = Taskify.Models.Models.TaskStatus;
namespace Taskify.Services.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<TaskStatus>> GetAllStatusesAsync();
    }
}