using Domain;
using Domain.Entities;
using Domain.ResourceParameters;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBoardService
    {
        Task<bool> SaveChangesAsync();
        Task<Board> AddBoardAsync(Board board);
        Task<PagedList<Board>> GetBoardsAsync(BoardsResourceParameters resourceParameters);
        Task<PagedList<Board>> GetUserBoardsAsync(BoardsResourceParameters resourceParameters, int userId);
        Task<Board> GetBoardAsync(int id);
        Task<bool> BoardExistsAsync(int id);
        Task DeleteBoardAsync(int id);
        Task<BoardWithContributors> GetBoardViewersAsync(int id);
        Task<BoardWithContributors> GetBoardEditorsAsync(int id);
    }
}