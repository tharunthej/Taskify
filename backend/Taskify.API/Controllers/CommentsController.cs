using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Taskify.DTO.CommentsDTO;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taskify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentsController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            var response = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
            return Ok(response);
        }

        // GET: api/comments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDto>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<CommentResponseDto>(comment);
            return Ok(response);
        }

        // POST: api/comments
        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> CreateComment([FromBody] CreateCommentDto createDto)
        {
            var newComment = _mapper.Map<Comment>(createDto);
            var createdComment = await _commentService.CreateCommentAsync(newComment);
            var response = _mapper.Map<CommentResponseDto>(createdComment);
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, response);
        }

        // PUT: api/comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto updateDto)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, comment);
            await _commentService.UpdateCommentAsync(comment);
            return NoContent();
        }

        // DELETE: api/comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
