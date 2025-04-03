using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.Models.Models;
using Taskify.DTO.TasksDTO;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskService taskService, 
            IMapper mapper,
            ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            try
            {
                var userId = GetCurrentUserId();
                var tasks = await _taskService.GetAllTasksAsync(userId);
                var response = _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var task = await _taskService.GetTaskByIdAsync(id, userId);
                var response = _mapper.Map<TaskResponseDto>(task);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting task {id}");
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto createDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var newTask = _mapper.Map<TaskItem>(createDto);
                var createdTask = await _taskService.CreateTaskAsync(newTask, userId);
                var response = CreatedAtAction(
                    nameof(GetTask), 
                    new { id = createdTask.Id }, 
                    _mapper.Map<TaskResponseDto>(createdTask));
                return response;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex.Message);
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "An error occurred while creating the task");
            }
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var task = _mapper.Map<TaskItem>(updateDto);
                task.Id = id; // Ensure ID matches route
                
                await _taskService.UpdateTaskAsync(task, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task {id}");
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _taskService.DeleteTaskAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task {id}");
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("Invalid user credentials");
            }
            return int.Parse(userIdClaim);
        }
    }
}