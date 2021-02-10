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
        public ChatHub(AmongUsContext db, LiveUsersMenager mng)
        {
            dbContext = db;
            userMenager = mng;
        }        
        public Task SendMesageToUser(string userId , string message)
        {
            string connectionId = userMenager.LiveUsers.GetValueOrDefault(userId);
            if(connectionId == null)
                return null;
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public override Task OnConnectedAsync()
        {
            userMenager.LiveUsers.Add(Context.UserIdentifier, Context.ConnectionId);
            Clients.All.SendAsync("UserConnected", Context.UserIdentifier);
            List<string> liveUsers = new List<string>();
            foreach(var el in userMenager.LiveUsers)
                liveUsers.Add(el.Key);
            Clients.Caller.SendAsync("GetLiveUsers", new JsonResult(new {userList = liveUsers}));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            userMenager.LiveUsers.Remove(Context.UserIdentifier);
            Clients.All.SendAsync("UserDiconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}