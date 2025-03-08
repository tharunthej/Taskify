using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.ProjectsDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        
        public ProjectsController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var response = _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
            return Ok(response);
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<ProjectResponseDto>(project);
            return Ok(response);
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromBody] CreateProjectDto createDto)
        {
            var newProject = _mapper.Map<Project>(createDto);
            var createdProject = await _projectService.CreateProjectAsync(newProject);
            var response = _mapper.Map<ProjectResponseDto>(createdProject);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, response);
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto updateDto)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _mapper.Map(updateDto, project);
            await _projectService.UpdateProjectAsync(project);
            return NoContent();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }

        // POST: api/projects/1/members/2
        [HttpPost("{projectId}/members/{userId}")]
        public async Task<IActionResult> AddMember(int projectId, int userId)
        {
            await _projectService.AddMemberToProjectAsync(projectId, userId);
            return NoContent();
        }
    }
}
