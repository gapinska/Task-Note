using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ResourceParameters;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly DataContext _context;
        private readonly ISortHelper<Board> _sortHelper;

        public BoardRepository(DataContext context, ISortHelper<Board> sortHelper)
        {
            _context = context;
            _sortHelper = sortHelper;
        }

        public async Task<Board> AddBoardAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
            return board;
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await GetBoardAsync(id);
            _context.Boards.Remove(board);
            _context.RemoveRange(_context.Quests.Where(q => q.BoardId == id));
        }

        public async Task<Board> GetBoardAsync(int id)
        {
            var board = await _context.Boards.SingleOrDefaultAsync(b => b.Id == id);
            return board;
        }

        public async Task<PagedList<Board>> GetBoardsAsync(BoardsResourceParameters resourceParameters)
        {
            var collection = _context.Boards as IQueryable<Board>;

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                var searchQuery = resourceParameters.SearchQuery.Trim();
                collection = collection.Where(u => u.Name.Contains(searchQuery,
                                                       StringComparison.OrdinalIgnoreCase));
            }

            var sortedBoards = _sortHelper.ApplySort(collection, resourceParameters.OrderBy);

            var collectionToReturn = await PagedList<Board>
                .ToPagedListAsync(sortedBoards, resourceParameters.PageNumber, resourceParameters.PageSize);

            return collectionToReturn;
        }

        public async Task<PagedList<Board>> GetUserBoardsAsync(BoardsResourceParameters resourceParameters, int userId)
        {
            var collection = _context.Boards.Where(x => x.UserId == userId);
            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                var searchQuery = resourceParameters.SearchQuery.Trim();
                collection = collection.Where(u => u.Name.Contains(searchQuery,
                                                       StringComparison.OrdinalIgnoreCase));
            }

            var sortedBoards = _sortHelper.ApplySort(collection, resourceParameters.OrderBy);

            var collectionToReturn = await PagedList<Board>
                .ToPagedListAsync(sortedBoards, resourceParameters.PageNumber, resourceParameters.PageSize);

            return collectionToReturn;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> BoardExistsAsync(int id)
        {
            return await _context.Boards.AnyAsync(b => b.Id == id);
        }

        public async Task<BoardWithContributors> GetBoardViewersAsync(int id)
        {
            if (await BoardExistsAsync(id))
            {
                var boardViewers = await _context.BoardViewers.Where(bv => bv.BoardId == id)
                    .Select(board => board.ViewerId).ToListAsync();
                
                return new BoardWithContributors() 
                {
                    BoardId = id,
                    ContributorsIds = boardViewers 
                };
            }

            return new BoardWithContributors();
        }

        public async Task<BoardWithContributors> GetBoardEditorsAsync(int id)
        {
            if (await BoardExistsAsync(id))
            {
                var boardEditors = await _context.BoardEditors.Where(bv => bv.BoardId == id)
                    .Select(board => board.EditorId).ToListAsync();

                return new BoardWithContributors()
                {
                    BoardId = id,
                    ContributorsIds = boardEditors
                };
            }

            return new BoardWithContributors();
        }
    }
}