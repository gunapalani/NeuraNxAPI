using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeuraNx.Models.DTOs;
using NeuraNx.Models.Enums;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interface;
using NeuraNx.Repository.Interfaces;
using NeuraNx.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTaskStatus = NeuraNx.Repository.Enums.TaskStatus;
using ModelsTaskStatus = NeuraNx.Models.Enums.TaskStatus;

namespace NeuraNx.Service.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IEmployeesRepository _employeeRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public TaskService(
            ITaskRepository taskRepository,
            IBoardRepository boardRepository,
            IEmployeesRepository employeeRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _boardRepository = boardRepository;
            _employeeRepository = employeeRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return await MapTasksToResponseDtosAsync(tasks);
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByBoardIdAsync(Guid boardId)
        {
            var tasks = await _taskRepository.GetByBoardIdAsync(boardId);
            return await MapTasksToResponseDtosAsync(tasks);
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByAssignedToIdAsync(Guid employeeId)
        {
            var tasks = await _taskRepository.GetByAssignedToIdAsync(employeeId);
            return await MapTasksToResponseDtosAsync(tasks);
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return null;

            return await MapTaskToResponseDtoAsync(task);
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            // Validate board exists
            if (!await _boardRepository.ExistsAsync(createTaskDto.BoardId))
            {
                throw new KeyNotFoundException($"Board with ID {createTaskDto.BoardId} not found.");
            }

            // Validate employee exists (if assigned)
            if (createTaskDto.AssignedToId.HasValue &&
                !await _employeeRepository.ExistsAsync(createTaskDto.AssignedToId.Value))
            {
                throw new KeyNotFoundException($"Employee with ID {createTaskDto.AssignedToId.Value} not found.");
            }

            var taskItem = _mapper.Map<TaskItem>(createTaskDto);
            taskItem.Status = RepositoryTaskStatus.Todo; // Default status

            var createdTask = await _taskRepository.AddAsync(taskItem);

            // Reload with navigation properties
            var taskWithDetails = await _taskRepository.GetByIdAsync(createdTask.Id);
            return await MapTaskToResponseDtoAsync(taskWithDetails!);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            // Validate employee exists (if assigned)
            if (updateTaskDto.AssignedToId.HasValue &&
                !await _employeeRepository.ExistsAsync(updateTaskDto.AssignedToId.Value))
            {
                throw new KeyNotFoundException($"Employee with ID {updateTaskDto.AssignedToId.Value} not found.");
            }

            // Check concurrency
            if (!existingTask.RowVersion?.SequenceEqual(updateTaskDto.RowVersion) == true)
            {
                throw new DbUpdateConcurrencyException("The task has been modified by another user. Please refresh and try again.");
            }

            _mapper.Map(updateTaskDto, existingTask);

            try
            {
                var updatedTask = await _taskRepository.UpdateAsync(existingTask);

                // Reload with navigation properties
                var taskWithDetails = await _taskRepository.GetByIdAsync(updatedTask.Id);
                return await MapTaskToResponseDtoAsync(taskWithDetails!);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("The task has been modified by another user. Please refresh and try again.");
            }
        }

        public async Task<TaskResponseDto> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusDto updateTaskStatusDto)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            // Check concurrency
            if (!existingTask.RowVersion?.SequenceEqual(updateTaskStatusDto.RowVersion) == true)
            {
                throw new DbUpdateConcurrencyException("The task has been modified by another user. Please refresh and try again.");
            }

            existingTask.Status = (RepositoryTaskStatus)updateTaskStatusDto.Status;

            try
            {
                var updatedTask = await _taskRepository.UpdateAsync(existingTask);

                // Reload with navigation properties
                var taskWithDetails = await _taskRepository.GetByIdAsync(updatedTask.Id);
                return await MapTaskToResponseDtoAsync(taskWithDetails!);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("The task has been modified by another user. Please refresh and try again.");
            }
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            if (!await _taskRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            await _taskRepository.DeleteAsync(id);
        }

        public async Task<bool> TaskExistsAsync(Guid id)
        {
            return await _taskRepository.ExistsAsync(id);
        }

        private async Task<IEnumerable<TaskResponseDto>> MapTasksToResponseDtosAsync(IEnumerable<TaskItem> tasks)
        {
            var responseDtos = new List<TaskResponseDto>();

            foreach (var task in tasks)
            {
                var responseDto = await MapTaskToResponseDtoAsync(task);
                responseDtos.Add(responseDto);
            }

            return responseDtos;
        }

        private async Task<TaskResponseDto> MapTaskToResponseDtoAsync(TaskItem task)
        {
            var responseDto = _mapper.Map<TaskResponseDto>(task);
            responseDto.CommentCount = await _commentRepository.GetCountByTaskItemIdAsync(task.Id);
            return responseDto;
        }
    }
}