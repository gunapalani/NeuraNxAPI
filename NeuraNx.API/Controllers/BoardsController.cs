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
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllBoards()
        {
            try
            {
                var boards = await _boardService.GetAllBoardsAsync();
                return Ok(boards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving boards.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardById(Guid id)
        {
            try
            {
                var board = await _boardService.GetBoardByIdAsync(id);
                if (board == null)
                {
                    return NotFound(new { message = $"Board with ID {id} not found." });
                }

                return Ok(board);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the board.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] CreateBoardDto createBoardDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBoard = await _boardService.CreateBoardAsync(createBoardDto);
                return CreatedAtAction(nameof(GetBoardById), new { id = createdBoard.Id }, createdBoard);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the board.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BoardDto>> UpdateBoard(Guid id, [FromBody] UpdateBoardDto updateBoardDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedBoard = await _boardService.UpdateBoardAsync(id, updateBoardDto);
                return Ok(updatedBoard);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the board.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBoard(Guid id)
        {
            try
            {
                await _boardService.DeleteBoardAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the board.", error = ex.Message });
            }
        }
    }
}