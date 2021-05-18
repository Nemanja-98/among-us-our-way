using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AmongUs_OurWay.DataManagement;

namespace AmongUs_OurWay.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private IUserRepository _repository;
        private LiveUsersMenager _userMenager;
        public ChatHub(Repository repo, LiveUsersMenager mngU)
        {
            _repository = repo.GetUserRepository();
            _userMenager = mngU;
        }        
        public async Task SendMessageToUser(string userId , string message, string sentTime)
        {
            string connectionId = _userMenager.LiveUsers.GetValueOrDefault(userId);
            if(connectionId == null)
                return;
            Message entry =  new Message{
                UserSent = Context.UserIdentifier,
                UserReceived = userId,
                Content = message,
                SentTime = sentTime
            };
            await _repository.AddMessage(entry);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message, sentTime);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if(!_userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                _userMenager.LiveUsers.Add(Context.UserIdentifier, Context.ConnectionId);
            else
                return null;
            await Clients.Others.SendAsync("UserConnected", Context.UserIdentifier);
            List<string> liveUsers = new List<string>();
            foreach(var el in _userMenager.LiveUsers)
                liveUsers.Add(el.Key);
            await Clients.Caller.SendAsync("GetLiveUsers", new JsonResult(new {userList = liveUsers}));
            Console.WriteLine("Connected " + Context.UserIdentifier + " " + DateTime.Now.TimeOfDay.ToString());
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            if(_userMenager.LiveUsers.ContainsKey(Context.UserIdentifier))
                _userMenager.LiveUsers.Remove(Context.UserIdentifier);
            else
                return null;
            await Clients.All.SendAsync("UserDisconnected", Context.UserIdentifier);
            Console.WriteLine("Disconnected " + Context.UserIdentifier + " " + DateTime.Now.TimeOfDay.ToString());
            return base.OnDisconnectedAsync(exception);
        }
    }
}