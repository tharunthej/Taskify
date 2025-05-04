using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.ProjectsDTO;
using Taskify.DTO.ProjectMembersDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(
            IProjectService projectService, 
            IMapper mapper,
            ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            try
            {
                var projects = await _projectService.GetAllProjectsAsync(GetCurrentUserId());
                var response = _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting projects");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id, GetCurrentUserId());
                var response = _mapper.Map<ProjectResponseDto>(project);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting project {id}");
                return StatusCode(500, "An error occurred here");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromBody] CreateProjectDto createDto)
        {
            try
            {
                var project = _mapper.Map<Project>(createDto);
                // Add creator info from authenticated user
                project.CreatedBy = GetCurrentUserId(); 
                project.CreatedAt = DateTime.UtcNow;
        
                var createdProject = await _projectService.CreateProjectAsync(project);
                var response = CreatedAtAction(nameof(GetProject), 
                    new { id = createdProject.Id }, 
                    _mapper.Map<ProjectResponseDto>(createdProject));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto updateDto)
        {
            try
            {
                var project = _mapper.Map<Project>(updateDto);
                project.Id = id;
                await _projectService.UpdateProjectAsync(project, GetCurrentUserId());
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating project {id}");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id, GetCurrentUserId());
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting project {id}");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpPost("{projectId}/members/{userId}")]
        public async Task<IActionResult> AddMember(int projectId, int userId)
        {
            try
            {
                await _projectService.AddMemberToProjectAsync(projectId, userId, GetCurrentUserId());
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding member to project {projectId}");
                return StatusCode(500, "An error occurred");
            }
        }

        [HttpGet("{projectId}/members")]
        public async Task<ActionResult<IEnumerable<ProjectMemberResponseDto>>> GetProjectMembers(int projectId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var members = await _projectService.GetProjectMembersAsync(projectId, userId);
                var response = _mapper.Map<IEnumerable<ProjectMemberResponseDto>>(members);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
