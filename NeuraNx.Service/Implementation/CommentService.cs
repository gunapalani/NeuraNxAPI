using AutoMapper;
using NeuraNx.Models.DTOs;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interface;
using NeuraNx.Repository.Interfaces;
using NeuraNx.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            ITaskRepository taskRepository,
            IEmployeesRepository employeeRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(Guid taskItemId)
        {
            var comments = await _commentRepository.GetByTaskItemIdAsync(taskItemId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto?> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                return null;

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            // Validate that the task exists
            if (!await _taskRepository.ExistsAsync(createCommentDto.TaskItemId))
            {
                throw new KeyNotFoundException($"Task with ID {createCommentDto.TaskItemId} not found.");
            }

            // Validate that the employee exists
            if (!await _employeeRepository.ExistsAsync(createCommentDto.CreatedById))
            {
                throw new KeyNotFoundException($"Employee with ID {createCommentDto.CreatedById} not found.");
            }

            var comment = _mapper.Map<Comment>(createCommentDto);
            var createdComment = await _commentRepository.AddAsync(comment);
            
            // Reload with navigation properties
            var commentWithDetails = await _commentRepository.GetByIdAsync(createdComment.Id);
            return _mapper.Map<CommentDto>(commentWithDetails!);
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto updateCommentDto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {id} not found.");
            }

            _mapper.Map(updateCommentDto, existingComment);
            var updatedComment = await _commentRepository.UpdateAsync(existingComment);
            
            // Reload with navigation properties
            var commentWithDetails = await _commentRepository.GetByIdAsync(updatedComment.Id);
            return _mapper.Map<CommentDto>(commentWithDetails!);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            if (!await _commentRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Comment with ID {id} not found.");
            }

            await _commentRepository.DeleteAsync(id);
        }

        public async Task<bool> CommentExistsAsync(Guid id)
        {
            return await _commentRepository.ExistsAsync(id);
        }
    }
}