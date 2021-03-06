using Domain;
using Domain.Entities;
using Domain.ResourceParameters;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBoardRepository
    {
        Task<bool> SaveChangesAsync();
        Task<Board> AddBoardAsync(Board board);
        Task<PagedList<Board>> GetBoardsAsync(BoardsResourceParameters resourceParameters);
        Task<Board> GetBoardAsync(int id);
        Task<BoardWithContributors> GetBoardViewersAsync(int id);
        Task<BoardWithContributors> GetBoardEditorsAsync(int id);
        Task<bool> BoardExistsAsync(int id);
        Task DeleteBoardAsync(int id);
        Task<PagedList<Board>> GetUserBoardsAsync(BoardsResourceParameters boardsResourceParameters, int userId);
    }
}