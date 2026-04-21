using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NeuraNx.Models.DTOs;
using NeuraNx.Repository.Entities;
using NeuraNx.Repository.Interfaces;
using NeuraNx.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service.Implementation
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public BoardService(IBoardRepository boardRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _boardRepository = boardRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BoardDto>> GetAllBoardsAsync()
        {
            var boards = await _boardRepository.GetAllAsync();
            var boardDtos = new List<BoardDto>();

            foreach (var board in boards)
            {
                var boardDto = _mapper.Map<BoardDto>(board);
                boardDto.TaskCount = await _taskRepository.GetCountByBoardIdAsync(board.Id);
                boardDtos.Add(boardDto);
            }

            return boardDtos;
        }

        public async Task<BoardDto?> GetBoardByIdAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board == null)
                return null;

            var boardDto = _mapper.Map<BoardDto>(board);
            boardDto.TaskCount = await _taskRepository.GetCountByBoardIdAsync(board.Id);
            return boardDto;
        }

        public async Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto)
        {
            // Validate unique name
            if (await _boardRepository.ExistsByNameAsync(createBoardDto.Name))
            {
                throw new InvalidOperationException($"A board with name '{createBoardDto.Name}' already exists.");
            }

            var board = _mapper.Map<Board>(createBoardDto);
            var createdBoard = await _boardRepository.AddAsync(board);
            
            var boardDto = _mapper.Map<BoardDto>(createdBoard);
            boardDto.TaskCount = 0;
            return boardDto;
        }

        public async Task<BoardDto> UpdateBoardAsync(Guid id, UpdateBoardDto updateBoardDto)
        {
            var existingBoard = await _boardRepository.GetByIdAsync(id);
            if (existingBoard == null)
            {
                throw new KeyNotFoundException($"Board with ID {id} not found.");
            }

            // Validate unique name (excluding current board)
            if (await _boardRepository.ExistsByNameAsync(updateBoardDto.Name, id))
            {
                throw new InvalidOperationException($"A board with name '{updateBoardDto.Name}' already exists.");
            }

            _mapper.Map(updateBoardDto, existingBoard);
            var updatedBoard = await _boardRepository.UpdateAsync(existingBoard);
            
            var boardDto = _mapper.Map<BoardDto>(updatedBoard);
            boardDto.TaskCount = await _taskRepository.GetCountByBoardIdAsync(updatedBoard.Id);
            return boardDto;
        }

        public async Task DeleteBoardAsync(Guid id)
        {
            if (!await _boardRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Board with ID {id} not found.");
            }

            await _boardRepository.DeleteAsync(id);
        }

        public async Task<bool> BoardExistsAsync(Guid id)
        {
            return await _boardRepository.ExistsAsync(id);
        }
    }
}