using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.DataManagement
{
    public interface IGameRepository
    {
        Game GetGame(int gameId);

        ServerResponse AddAction(PlayerAction action);

        ServerResponse AddPlayer(GameHistory gameHistory);

        ServerResponse AddGame(Game game);
    }
}