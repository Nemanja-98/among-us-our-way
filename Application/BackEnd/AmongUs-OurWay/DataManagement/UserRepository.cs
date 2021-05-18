using System.Collections.Generic;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AmongUs_OurWay.DataManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly AmongUsContext _dbContext;

        public UserRepository(AmongUsContext db)
        {
            _dbContext = db;
        }

        public async Task<ServerResponse> AddAction(PlayerAction action)
        {
            await _dbContext.PlayerActions.AddAsync(action);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddFriend(Friend friend)
        {
            Friend reverseFriend = new Friend{
                User1Ref = friend.User2Ref,
                User2Ref = friend.User1Ref};
            await _dbContext.Friends.AddAsync(friend);
            await _dbContext.Friends.AddAsync(reverseFriend);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddGame(GameHistory game, string callerUsername)
        {
            User user = await _dbContext.Users.FindAsync(game.UserId);
            if(user == null)
                return ServerResponse.NotFound;
            if(callerUsername != user.Username)
                return ServerResponse.Unauthorized;
            await _dbContext.GameHistorys.AddAsync(game);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddMessage(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> AddRequest(PendingRequest pendingRequest)
        {
            await _dbContext.PendingRequests.AddAsync(pendingRequest);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ServerResponse> DeleteRequest(string requestId, string username)
        {
            PendingRequest pendingRequest = await _dbContext.PendingRequests.FindAsync(requestId);
            if(pendingRequest == null)
                return ServerResponse.NotFound;
            if(username != pendingRequest.UserSentRef || username != pendingRequest.UserReceivedRef)
                return ServerResponse.Unauthorized;
            _dbContext.PendingRequests.Remove(pendingRequest);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<ActionResult> GetMessages(User userSent, User userReceived)
        {
            List<Message> result = new List<Message>();
            foreach(Message m in _dbContext.Messages)
            {
                if(m.UserSent == userSent.Username && m.UserReceived == userReceived.Username)
                {
                    result.Add(m);
                    continue;
                }
                if(m.UserReceived == userSent.Username && m.UserSent == userReceived.Username)
                    result.Add(m);
            }
            return new JsonResult(new {messages = result});
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User user = await _dbContext.Users.FindAsync(username);
            if(user == null)
                return null;
            return user;
        }

        public async Task<ServerResponse> SaveUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<List<User>> Search(string substr)
        {
            List<User> result = new List<User>();
            foreach(User u in _dbContext.Users.ToList())
                if(u.Username.Contains(substr))
                    result.Add(u);
            return result;
        }

        public async Task<ServerResponse> UpdateUser(User user)
        {
            user.Password = user.Password;
            user.GamesPlayed = user.GamesPlayed;
            user.CrewmateGames = user.CrewmateGames;
            user.ImpostorGames = user.ImpostorGames;
            user.CrewmateWonGames = user.CrewmateWonGames;
            user.ImpostorWonGames = user.ImpostorWonGames;
            user.TasksCompleted = user.TasksCompleted;
            user.AllTasksCompleted = user.AllTasksCompleted;
            user.Kills = user.Kills;
            await _dbContext.SaveChangesAsync();
            return ServerResponse.Ok;
        }

        public async Task<List<User>> UserList()
        {
            return _dbContext.Users.ToList();
        }
    }
}