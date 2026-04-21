using Microsoft.EntityFrameworkCore;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByTaskItemIdAsync(Guid taskItemId)
        {
            return await _context.Comments
                .Include(c => c.CreatedBy)
                .Where(c => c.TaskItemId == taskItemId)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.CreatedBy)
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            comment.Id = Guid.NewGuid();
            comment.CreatedOn = DateTimeOffset.UtcNow;
            comment.UpdatedOn = DateTimeOffset.UtcNow;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            comment.UpdatedOn = DateTimeOffset.UtcNow;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<int> GetCountByTaskItemIdAsync(Guid taskItemId)
        {
            return await _context.Comments.CountAsync(c => c.TaskItemId == taskItemId);
        }
    }
}