using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.ProjectMembersDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;
        private readonly IMapper _mapper;

        public ProjectMembersController(IProjectMemberService projectMemberService, IMapper mapper)
        {
            _projectMemberService = projectMemberService;
            _mapper = mapper;
        }

        // GET: api/projectmembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectMemberResponseDto>>> GetProjectMembers()
        {
            var members = await _projectMemberService.GetAllProjectMembersAsync();
            var response = _mapper.Map<IEnumerable<ProjectMemberResponseDto>>(members);
            return Ok(response);
        }

        // GET: api/projectmembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectMemberResponseDto>> GetProjectMember(int id)
        {
            var member = await _projectMemberService.GetProjectMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<ProjectMemberResponseDto>(member);
            return Ok(response);
        }

        // POST: api/projectmembers
        [HttpPost]
        public async Task<ActionResult<ProjectMemberResponseDto>> CreateProjectMember([FromBody] CreateProjectMemberDto createDto)
        {
            var newMember = _mapper.Map<ProjectMember>(createDto);
            var createdMember = await _projectMemberService.CreateProjectMemberAsync(newMember);
            var response = _mapper.Map<ProjectMemberResponseDto>(createdMember);
            return CreatedAtAction(nameof(GetProjectMember), new { id = createdMember.Id }, response);
        }

        // PUT: api/projectmembers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectMember(int id, [FromBody] CreateProjectMemberDto updateDto)
        {
            var existingMember = await _projectMemberService.GetProjectMemberByIdAsync(id);
            if (existingMember == null)
            {
                return NotFound();
            }
            // Reuse the same DTO for update (or create a dedicated UpdateProjectMemberDto if needed).
            _mapper.Map(updateDto, existingMember);
            await _projectMemberService.UpdateProjectMemberAsync(existingMember);
            return NoContent();
        }

        // DELETE: api/projectmembers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectMember(int id)
        {
            await _projectMemberService.DeleteProjectMemberAsync(id);
            return NoContent();
        }
    }
}
