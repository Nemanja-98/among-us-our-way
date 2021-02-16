using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmongUs_OurWay.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private AmongUsContext dbContext;
        private LiveUsersMenager userMenager;
        public ChatHub(AmongUsContext db, LiveUsersMenager mngU)
        {
            dbContext = db;
            userMenager = mngU;
        }        
        public async Task SendMesageToUser(string userId , string message, string sentTime)
        {
            string connectionId = userMenager.LiveUsers.GetValueOrDefault(userId);
            if(connectionId == null)
                return;
            Message entry =  new Message{
                UserSent = Context.UserIdentifier,
                UserReceived = userId,
                Content = message,
                SentTime = sentTime
            };
            dbContext.Messages.Add(entry);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if(!userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                userMenager.LiveUsers.Add(Context.UserIdentifier, Context.ConnectionId);
            await Clients.Others.SendAsync("UserConnected", Context.UserIdentifier);
            List<string> liveUsers = new List<string>();
            foreach(var el in userMenager.LiveUsers)
                liveUsers.Add(el.Key);
            await Clients.Caller.SendAsync("GetLiveUsers", new JsonResult(new {userList = liveUsers}));
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            if(!userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                userMenager.LiveUsers.Remove(Context.UserIdentifier);
            await Clients.All.SendAsync("UserDisconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}