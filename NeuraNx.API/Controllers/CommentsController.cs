using Microsoft.AspNetCore.Mvc;
using NeuraNx.Models.DTOs;
using NeuraNx.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Get all comments for a specific task
        /// </summary>
        /// <param name="taskId">Task ID to get comments for</param>
        /// <returns>List of comments for the task</returns>
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByTaskId(Guid taskId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving comments.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a comment by ID
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>Comment details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new { message = $"Comment with ID {id} not found." });
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the comment.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="createCommentDto">Comment creation data</param>
        /// <returns>Created comment</returns>
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdComment = await _commentService.CreateCommentAsync(createCommentDto);
                return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, createdComment);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the comment.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing comment
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <param name="updateCommentDto">Comment update data</param>
        /// <returns>Updated comment</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(Guid id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedComment = await _commentService.UpdateCommentAsync(id, updateCommentDto);
                return Ok(updatedComment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the comment.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id">Comment ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the comment.", error = ex.Message });
            }
        }
    }
}