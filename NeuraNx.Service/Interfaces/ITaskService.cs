using NeuraNx.Models.DTOs;
using NeuraNx.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskResponseDto>> GetTasksByBoardIdAsync(Guid boardId);
        Task<IEnumerable<TaskResponseDto>> GetTasksByAssignedToIdAsync(Guid employeeId);
        Task<TaskResponseDto?> GetTaskByIdAsync(Guid id);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto);
        Task<TaskResponseDto> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto updateTaskStatusDto);
        Task DeleteTaskAsync(Guid id);
        Task<bool> TaskExistsAsync(Guid id);
    }
}