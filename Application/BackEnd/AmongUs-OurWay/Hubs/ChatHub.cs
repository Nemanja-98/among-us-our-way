using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AmongUs_OurWay.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMesageToUser(string connectionId , string message)
        {
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public Task JoinGroup(string groupId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public Task MessageAlive(string groupId, string message)
        {
            return Clients.Group(groupId).SendAsync("ReceiveInGameMessage", Context.UserIdentifier, message);
        }

        public Task MessageDead(string groupId, string message)
        {
            return Clients.Group(groupId).SendAsync("ReceiveInGameMessage", Context.UserIdentifier, message);
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("UserConnected", Context.UserIdentifier, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.SendAsync("UserDiconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}