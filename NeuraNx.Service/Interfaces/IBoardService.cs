using NeuraNx.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Interfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardDto>> GetAllBoardsAsync();
        Task<BoardDto?> GetBoardByIdAsync(Guid id);
        Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto);
        Task<BoardDto> UpdateBoardAsync(Guid id, UpdateBoardDto updateBoardDto);
        Task DeleteBoardAsync(Guid id);
        Task<bool> BoardExistsAsync(Guid id);
    }
}