using NeuraNx.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByTaskIdAsync(Guid taskItemId);
        Task<CommentDto?> GetCommentByIdAsync(Guid id);
        Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto);
        Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentDto updateCommentDto);
        Task DeleteCommentAsync(Guid id);
        Task<bool> CommentExistsAsync(Guid id);
    }
}