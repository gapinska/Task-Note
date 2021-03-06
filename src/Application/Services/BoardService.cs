using System.Threading.Tasks;
using Application.Interfaces;
using Domain.ResourceParameters;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public Task<Board> AddBoardAsync(Board board)
        {
            var boardFromRepo = _boardRepository.AddBoardAsync(board);

            return boardFromRepo;
        }

        public async Task DeleteBoardAsync(int id)
        {
            await _boardRepository.DeleteBoardAsync(id);
        }

        public async Task<PagedList<Board>> GetBoardsAsync(BoardsResourceParameters resourceParameters)
        {
            var boards = await _boardRepository.GetBoardsAsync(resourceParameters);

            return boards;
        }

        public async Task<Board> GetBoardAsync(int id)
        {
            var board = await _boardRepository.GetBoardAsync(id);

            return board;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _boardRepository.SaveChangesAsync();
        }

        public async Task<bool> BoardExistsAsync(int id)
        {
            return await _boardRepository.BoardExistsAsync(id);
        }

        public async Task<BoardWithContributors> GetBoardViewersAsync(int id)
        {
            return await _boardRepository.GetBoardViewersAsync(id);
        }

        public async Task<BoardWithContributors> GetBoardEditorsAsync(int id)
        {
            return await _boardRepository.GetBoardEditorsAsync(id);
        }

        public async Task<PagedList<Board>> GetUserBoardsAsync(BoardsResourceParameters resourceParameters, int userId)
        {
            return await _boardRepository.GetUserBoardsAsync(resourceParameters, userId);
        }
    }
}