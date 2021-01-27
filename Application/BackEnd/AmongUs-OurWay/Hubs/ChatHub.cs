using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace AmongUs_OurWay.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public static List<UserConnectionModel> ActiveUsers { get; set; }
        
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
            ActiveUsers.Add(new UserConnectionModel{Username = Context.UserIdentifier, ConnectionId = Context.ConnectionId});
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