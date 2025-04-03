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

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                    .ThenInclude(p => p.Members)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .AsNoTracking()
                .Where(t => CanAccessTask(t, userId))
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                    .ThenInclude(p => p.Members)
                .Include(t => t.Assignee)
                .Include(t => t.Creator)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) 
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            if (!CanAccessTask(task, userId))
                throw new UnauthorizedAccessException("You don't have permission to access this task");

            return task;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                // Verify project admin permissions
                var project = await _context.Projects
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == task.ProjectId);

                if (project == null)
                    throw new KeyNotFoundException("Project not found");

                if (!IsProjectAdmin(project, userId))
                    throw new UnauthorizedAccessException("Only project admins can create tasks");

                task.CreatedBy = userId;
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

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingTask = await _context.Tasks
                    .Include(t => t.Project)
                        .ThenInclude(p => p.Members)
                    .FirstOrDefaultAsync(t => t.Id == task.Id);

                if (existingTask == null)
                    throw new KeyNotFoundException("Task not found");

                if (!CanEditTask(existingTask, userId))
                    throw new UnauthorizedAccessException("You don't have permission to edit this task");

                _context.Entry(existingTask).CurrentValues.SetValues(task);
                await _context.SaveChangesAsync();
                
                var updatedTaskDto = _mapper.Map<TaskResponseDto>(existingTask);
                await _hubContext.Clients.All.SendAsync("TaskUpdated", updatedTaskDto);
                
                await transaction.CommitAsync();
                return existingTask;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTaskAsync(int id, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var task = await _context.Tasks
                    .Include(t => t.Project)
                        .ThenInclude(p => p.Members)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (task == null)
                    throw new KeyNotFoundException("Task not found");

                if (!IsProjectAdmin(task.Project, userId))
                    throw new UnauthorizedAccessException("Only project admins can delete tasks");

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("TaskDeleted", id);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        #region Authorization Helpers
        private bool CanAccessTask(TaskItem task, int userId)
        {
            return task.AssignedTo == userId ||
                   IsProjectMember(task.Project, userId) ||
                   IsProjectAdmin(task.Project, userId) ||
                   HasGuestAccess(task.Project, userId);
        }

        private bool CanEditTask(TaskItem task, int userId)
        {
            return task.AssignedTo == userId || 
                   IsProjectAdmin(task.Project, userId);
        }

        private bool IsProjectAdmin(Project project, int userId)
        {
            return project.Members?.Any(m => 
                m.UserId == userId && m.UserRoleId == 1) ?? false;
        }

        private bool IsProjectMember(Project project, int userId)
        {
            return project.Members?.Any(m => 
                m.UserId == userId && m.UserRoleId == 2) ?? false;
        }

        private bool HasGuestAccess(Project project, int userId)
        {
            return project.Members?.Any(m => 
                m.UserId == userId && m.UserRoleId == 3) ?? false;
        }
        #endregion
    }
}