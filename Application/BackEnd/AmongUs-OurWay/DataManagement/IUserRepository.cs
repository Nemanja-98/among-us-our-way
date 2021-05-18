using System.Collections.Generic;
using System.Threading.Tasks;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmongUs_OurWay.DataManagement
{
    public interface IUserRepository
    {
        Task<List<User>> UserList();

        Task<List<User>> Search(string substr);

        Task<User> GetUserByUsername(string username);

        Task<ActionResult> GetMessages(User userSent, User userReceived);

        Task<ServerResponse> AddMessage(Message message);

        Task<ServerResponse> SaveUser(User user);

        Task<ServerResponse> AddGame(GameHistory game, string callerUsername);

        Task<ServerResponse> AddFriend(Friend friend);

        Task<ServerResponse> AddRequest(PendingRequest pendingRequest);

        Task<ServerResponse> AddAction(PlayerAction action);

        Task<ServerResponse> UpdateUser(User user);

        Task<ServerResponse> DeleteRequest(string requestId, string username);
    }
}