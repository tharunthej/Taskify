using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.Services.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly AppDbContext _context;

        public PriorityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskPriority>> GetAllPrioritiesAsync()
        {
            return await _context.TaskPriorities.ToListAsync();
        }

    }   
}
