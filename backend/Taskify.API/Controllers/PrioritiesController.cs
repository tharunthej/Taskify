using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.PriorityDTO;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritiesController : ControllerBase
    {
        private readonly IPriorityService _priorityService;
        private readonly IMapper _mapper;
        private readonly ILogger<StatusesController> _logger;

        public PrioritiesController(
            IPriorityService statusService,
            IMapper mapper,
            ILogger<StatusesController> logger)
        {
            _priorityService = statusService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskPrioritiesDto>>> GetAllStatuses()
        {
            try
            {
                var priorities = await _priorityService.GetAllPrioritiesAsync();
                var response = _mapper.Map<IEnumerable<TaskPrioritiesDto>>(priorities);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}