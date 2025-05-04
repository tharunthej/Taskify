using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.StatusDTO;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IMapper _mapper;
        private readonly ILogger<StatusesController> _logger;

        public StatusesController(
            IStatusService statusService,
            IMapper mapper,
            ILogger<StatusesController> logger)
        {
            _statusService = statusService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskStatusDto>>> GetAllStatuses()
        {
            try
            {
                var statuses = await _statusService.GetAllStatusesAsync();
                var response = _mapper.Map<IEnumerable<TaskStatusDto>>(statuses);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}