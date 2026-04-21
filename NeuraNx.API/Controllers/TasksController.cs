using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get all tasks with optional filtering by board
        /// </summary>
        /// <param name="boardId">Optional board ID to filter tasks</param>
        /// <returns>List of tasks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks([FromQuery] Guid? boardId = null)
        {
            try
            {
                IEnumerable<TaskResponseDto> tasks;

                if (boardId.HasValue)
                {
                    tasks = await _taskService.GetTasksByBoardIdAsync(boardId.Value);
                }
                else
                {
                    tasks = await _taskService.GetAllTasksAsync();
                }

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tasks.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get tasks assigned to a specific employee
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>List of tasks assigned to the employee</returns>
        [HttpGet("assigned-to/{employeeId}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksByAssignedEmployee(Guid employeeId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByAssignedToIdAsync(employeeId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving assigned tasks.", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a task by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Task details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound(new { message = $"Task with ID {id} not found." });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the task.", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="createTaskDto">Task creation data</param>
        /// <returns>Created task</returns>
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTask = await _taskService.CreateTaskAsync(createTaskDto);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the task.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="updateTaskDto">Task update data</param>
        /// <returns>Updated task</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTask = await _taskService.UpdateTaskAsync(id, updateTaskDto);
                return Ok(updatedTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the task.", error = ex.Message });
            }
        }

        /// <summary>
        /// Update task status only (for Kanban board movement)
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="updateTaskStatusDto">Task status update data</param>
        /// <returns>Updated task</returns>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusDto updateTaskStatusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTask = await _taskService.UpdateTaskStatusAsync(id, updateTaskStatusDto);
                return Ok(updatedTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the task status.", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the task.", error = ex.Message });
            }
        }
    }
}