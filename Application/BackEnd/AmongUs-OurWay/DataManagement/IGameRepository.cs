using System.Threading.Tasks;
using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.DataManagement
{
    public interface IGameRepository
    {
        Task<Game> GetGame(int gameId);

        Task<ServerResponse> AddAction(PlayerAction action);

        Task<ServerResponse> AddPlayer(GameHistory gameHistory);

        Task<ServerResponse> AddGame(Game game);
    }
}