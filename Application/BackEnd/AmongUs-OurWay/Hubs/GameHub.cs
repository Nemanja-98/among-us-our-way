using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AmongUs_OurWay.Hubs
{
    public class GameHub : Hub
    {
        private LiveGamesMenager gameMenager;
        private LiveUsersMenager userMenager;
        public GameHub(LiveGamesMenager mngG, LiveUsersMenager mngU)
        {
            gameMenager = mngG;
            userMenager = mngU;
        }

        [HubMethodName("joinGame")]
        public async Task JoinGroup(string roomId)
        {
            if(!userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                return;
            if(!gameMenager.LiveGames.Contains(roomId))
                return;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoinedGame", Context.UserIdentifier);
        }

        [HubMethodName("leaveGame")]
        public async Task LeaveGroup(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserLeftGame", Context.UserIdentifier);
        }

        public async Task Move(string roomId, int x, int y, int z, string direction)
        {
            if(!userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                return;
            await Clients.Group(roomId).SendAsync("Move", Context.UserIdentifier, x, y, z, direction);
        }

        public async Task Died(string roomId, int x, int y, int z)
        {
            if(!userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                return;
            await Clients.Group(roomId).SendAsync("Died", Context.UserIdentifier, x, y, z);
        }

        public override Task OnConnectedAsync()
        {
            if(!userMenager.InGameUsers.ContainsKey(Context.UserIdentifier))
                userMenager.InGameUsers.Add(Context.UserIdentifier, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if(!userMenager.InGameUsers.ContainsKey(Context.UserIdentifier))
                userMenager.InGameUsers.Remove(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}