using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.AttachmentsDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;

        public AttachmentsController(IAttachmentService attachmentService, IMapper mapper)
        {
            _attachmentService = attachmentService;
            _mapper = mapper;
        }

        // GET: api/attachments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttachmentResponseDto>>> GetAttachments()
        {
            var attachments = await _attachmentService.GetAllAttachmentsAsync();
            var response = _mapper.Map<IEnumerable<AttachmentResponseDto>>(attachments);
            return Ok(response);
        }

        // GET: api/attachments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AttachmentResponseDto>> GetAttachment(int id)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<AttachmentResponseDto>(attachment);
            return Ok(response);
        }

        // POST: api/attachments
        [HttpPost]
        public async Task<ActionResult<AttachmentResponseDto>> CreateAttachment([FromBody] CreateAttachmentDto createDto)
        {
            var newAttachment = _mapper.Map<Attachment>(createDto);
            var createdAttachment = await _attachmentService.CreateAttachmentAsync(newAttachment);
            var response = _mapper.Map<AttachmentResponseDto>(createdAttachment);
            return CreatedAtAction(nameof(GetAttachment), new { id = createdAttachment.Id }, response);
        }

        // PUT: api/attachments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttachment(int id, [FromBody] UpdateAttachmentDto updateDto)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, attachment);
            await _attachmentService.UpdateAttachmentAsync(attachment);
            return NoContent();
        }

        // DELETE: api/attachments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            await _attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }
    }
}
