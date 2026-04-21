using NeuraNx.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTaskStatus = NeuraNx.Repository.Enums.TaskStatus;

namespace NeuraNx.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<IEnumerable<TaskItem>> GetByBoardIdAsync(Guid boardId);
        Task<IEnumerable<TaskItem>> GetByAssignedToIdAsync(Guid employeeId);
        Task<IEnumerable<TaskItem>> GetByStatusAsync(RepositoryTaskStatus status);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem?> GetByIdWithDetailsAsync(Guid id);
        Task<TaskItem> AddAsync(TaskItem taskItem);
        Task<TaskItem> UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<int> GetCountByBoardIdAsync(Guid boardId);
    }
}