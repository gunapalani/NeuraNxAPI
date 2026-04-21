using NeuraNx.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository.Interfaces
{
    public interface IBoardRepository
    {
        Task<IEnumerable<Board>> GetAllAsync();
        Task<Board?> GetByIdAsync(Guid id);
        Task<Board?> GetByIdWithTasksAsync(Guid id);
        Task<Board> AddAsync(Board board);
        Task<Board> UpdateAsync(Board board);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name, Guid excludeId);
    }
}