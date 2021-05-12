using System.Collections.Generic;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmongUs_OurWay.DataManagement
{
    public interface IUserRepository
    {
        List<User> UserList();

        List<User> Search(string substr);

        User GetUserByUsername(string username);

        ActionResult GetMessages(User userSent, User userReceived);

        ServerResponse SaveUser(User user);

        ServerResponse AddGame(GameHistory game, string callerUsername);

        ServerResponse AddFriend(Friend friend);

        ServerResponse AddRequest(PendingRequest pendingRequest);

        ServerResponse AddAction(PlayerAction action);

        ServerResponse UpdateUser(User user);

        ServerResponse DeleteRequest(string requestId, string username);
    }
}