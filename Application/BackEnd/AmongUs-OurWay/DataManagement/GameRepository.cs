using System.Threading.Tasks;
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

        public async Task<ServerResponse> AddAction(PlayerAction action)
        {
            Game game = await _dbContext.Games.FindAsync(action.GameId);
            if(game == null)
                return ServerResponse.NotFound;
            //game.Actions.Add(action);
            await _dbContext.PlayerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddGame(Game game)
        {
            await _dbContext.Games.AddAsync(game);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddPlayer(GameHistory gameHistory)
        {
            Game game = await _dbContext.Games.FindAsync(gameHistory.GameId);
            if(game == null)
                return ServerResponse.NotFound;
            //game.Players.Add(gameHistory);
            await _dbContext.GameHistorys.AddAsync(gameHistory);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<Game> GetGame(int gameId)
        {
            Game game = await _dbContext.Games.FindAsync(gameId);
            if(game == null)
                return null;
            return game;
        }
    }
}