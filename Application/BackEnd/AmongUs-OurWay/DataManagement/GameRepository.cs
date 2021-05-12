using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.DataManagement
{
    public class GameRepository : IGameRepository
    {
        private readonly AmongUsContext _dbContext;

        public GameRepository(AmongUsContext db)
        {
            _dbContext = db;
        }

        public ServerResponse AddAction(PlayerAction action)
        {
            Game game = _dbContext.Games.Find(action.GameId);
            if(game == null)
                return ServerResponse.NotFound;
            game.Actions.Add(action);
            _dbContext.PlayerActions.Add(action);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse AddGame(Game game)
        {
            _dbContext.Games.Add(game);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse AddPlayer(GameHistory gameHistory)
        {
            Game game = _dbContext.Games.Find(gameHistory.GameId);
            if(game == null)
                return ServerResponse.NotFound;
            game.Players.Add(gameHistory);
            _dbContext.GameHistorys.Add(gameHistory);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public Game GetGame(int gameId)
        {
            Game game = _dbContext.Games.Find(gameId);
            if(game == null)
                return null;
            return game;
        }
    }
}