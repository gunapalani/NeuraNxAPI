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
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _context;

        public BoardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Board>> GetAllAsync()
        {
            return await _context.Boards
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Board?> GetByIdAsync(Guid id)
        {
            return await _context.Boards
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Board?> GetByIdWithTasksAsync(Guid id)
        {
            return await _context.Boards
                .Include(b => b.TaskItems)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Board> AddAsync(Board board)
        {
            board.Id = Guid.NewGuid();
            board.CreatedOn = DateTimeOffset.UtcNow;
            board.UpdatedOn = DateTimeOffset.UtcNow;

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task<Board> UpdateAsync(Board board)
        {
            board.UpdatedOn = DateTimeOffset.UtcNow;
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task DeleteAsync(Guid id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board != null)
            {
                _context.Boards.Remove(board);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Boards.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Boards.AnyAsync(b => b.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid excludeId)
        {
            return await _context.Boards.AnyAsync(b => b.Name.ToLower() == name.ToLower() && b.Id != excludeId);
        }
    }
}