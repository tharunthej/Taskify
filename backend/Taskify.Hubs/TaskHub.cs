using Microsoft.AspNetCore.SignalR;
using Taskify.Models.Models;

namespace Taskify.Hubs
{
    public class TaskHub : Hub
    {
        public async Task UpdateTask(TaskItem task)
        {
            await Clients.All.SendAsync("TaskUpdated", task);
        }
    }
}