using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;
using Taskify.DTO.TasksDTO;
using AutoMapper;
using Taskify.Hubs;

namespace Taskify.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<TaskHub> _hubContext;
        private readonly IMapper _mapper;

        public TaskService(
            AppDbContext context,
            IHubContext<TaskHub> hubContext,
            IMapper mapper)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return task ?? throw new KeyNotFoundException($"Task with ID {id} not found.");
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                
                var createdTaskDto = _mapper.Map<TaskResponseDto>(task);
                await _hubContext.Clients.All.SendAsync("TaskCreated", createdTaskDto);
                
                await transaction.CommitAsync();
                return task;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                var updatedTaskDto = _mapper.Map<TaskResponseDto>(task);
                await _hubContext.Clients.All.SendAsync("TaskUpdated", updatedTaskDto);
                
                await transaction.CommitAsync();
                return task;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task != null)
                {
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("TaskDeleted", id);
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}