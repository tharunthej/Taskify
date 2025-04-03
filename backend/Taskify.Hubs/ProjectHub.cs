using Microsoft.AspNetCore.SignalR;

namespace Taskify.Hubs
{
    public class ProjectHub : Hub
    {
        // Optional: Add methods if clients need to invoke server-side logic
        // Example: Join a project group to receive updates
        public async Task JoinProjectGroup(int projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Project-{projectId}");
        }
    }
}