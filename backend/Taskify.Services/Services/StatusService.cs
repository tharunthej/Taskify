using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Services.Interfaces;
using TaskStatus = Taskify.Models.Models.TaskStatus;

namespace Taskify.Services.Services
{
    public class StatusService : IStatusService
    {
        private readonly AppDbContext _context;

        public StatusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskStatus>> GetAllStatusesAsync()
        {
            return await _context.TaskStatuses.ToListAsync();
        }

    }   
}
