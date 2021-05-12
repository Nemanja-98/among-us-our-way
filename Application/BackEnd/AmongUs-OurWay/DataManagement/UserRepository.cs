using System.Collections.Generic;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AmongUs_OurWay.DataManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly AmongUsContext _dbContext;

        public UserRepository(AmongUsContext db)
        {
            _dbContext = db;
        }

        public ServerResponse AddAction(PlayerAction action)
        {
            _dbContext.PlayerActions.Add(action);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse AddFriend(Friend friend)
        {
            Friend reverseFriend = new Friend{
                User1Ref = friend.User2Ref,
                User2Ref = friend.User1Ref};
            _dbContext.Friends.Add(friend);
            _dbContext.Friends.Add(reverseFriend);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse AddGame(GameHistory game, string callerUsername)
        {
            User user = _dbContext.Users.Find(game.UserId);
            if(user == null)
                return ServerResponse.NotFound;
            if(callerUsername != user.Username)
                return ServerResponse.Unauthorized;
            _dbContext.GameHistorys.Add(game);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse AddRequest(PendingRequest pendingRequest)
        {
            _dbContext.PendingRequests.Add(pendingRequest);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ServerResponse DeleteRequest(string requestId, string username)
        {
            PendingRequest pendingRequest = _dbContext.PendingRequests.Find(requestId);
            if(pendingRequest == null)
                return ServerResponse.NotFound;
            if(username != pendingRequest.UserSentRef || username != pendingRequest.UserReceivedRef)
                return ServerResponse.Unauthorized;
            _dbContext.PendingRequests.Remove(pendingRequest);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public ActionResult GetMessages(User userSent, User userReceived)
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

        public User GetUserByUsername(string username)
        {
            User user = _dbContext.Users.Find(username);
            if(user == null)
                return null;
            return user;
        }

        public ServerResponse SaveUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public List<User> Search(string substr)
        {
            List<User> result = new List<User>();
            foreach(User u in _dbContext.Users.ToList())
                if(u.Username.Contains(substr))
                    result.Add(u);
            return result;
        }

        public ServerResponse UpdateUser(User user)
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
            _dbContext.SaveChanges();
            return ServerResponse.Ok;
        }

        public List<User> UserList()
        {
            return _dbContext.Users.ToList();
        }
    }
}