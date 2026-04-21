using Microsoft.EntityFrameworkCore;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTaskStatus = NeuraNx.Repository.Enums.TaskStatus;

namespace NeuraNx.Repository.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByBoardIdAsync(Guid boardId)
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .Where(t => t.BoardId == boardId)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByAssignedToIdAsync(Guid employeeId)
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId == employeeId)
                .OrderBy(t => t.Status)
                .ThenBy(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(RepositoryTaskStatus status)
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .Where(t => t.Status == status)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.TaskItems
                .Include(t => t.Board)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> AddAsync(TaskItem taskItem)
        {
            taskItem.Id = Guid.NewGuid();
            taskItem.CreatedOn = DateTimeOffset.UtcNow;
            taskItem.UpdatedOn = DateTimeOffset.UtcNow;

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem taskItem)
        {
            taskItem.UpdatedOn = DateTimeOffset.UtcNow;
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task DeleteAsync(Guid id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.TaskItems.AnyAsync(t => t.Id == id);
        }

        public async Task<int> GetCountByBoardIdAsync(Guid boardId)
        {
            return await _context.TaskItems.CountAsync(t => t.BoardId == boardId);
        }
    }
}