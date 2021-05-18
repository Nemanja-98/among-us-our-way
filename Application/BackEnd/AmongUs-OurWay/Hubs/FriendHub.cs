using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AmongUs_OurWay.DataManagement;

namespace AmongUs_OurWay.Hubs
{
    [Authorize]
    public class FriendHub : Hub
    {
        private IUserRepository _repository;
        private LiveUsersMenager _userMenager;
        public FriendHub(Repository repo, LiveUsersMenager mng)
        {
            _repository = repo.GetUserRepository();
            _userMenager = mng;
        }

        public async Task SendFriendRequest(string userSentRef, string userReceivedRef)
        {
            if(userSentRef == null || userReceivedRef == null)
                return;
            User userSent = await _repository.GetUserByUsername(userSentRef);
            User userRecieved = await _repository.GetUserByUsername(userReceivedRef);
            if((userSent == null) || (userRecieved == null))
                return;
            PendingRequest pendingRequest = new PendingRequest{
                UserSentRef = userSentRef,
                UserReceivedRef = userReceivedRef
            };
            await _repository.AddRequest(pendingRequest);

            string connectionId = _userMenager.LiveFriends.GetValueOrDefault(userReceivedRef);
            if(connectionId == null)
                return;
            await Clients.Client(connectionId).SendAsync("RequestReceived", userSent.Username);
        }

        public async Task AcceptFriendRequest(Friend friend)
        {
            if(friend.User1Ref == null || friend.User2Ref == null)
                return;
            User user1 = await _repository.GetUserByUsername(friend.User1Ref);
            User user2 = await _repository.GetUserByUsername(friend.User2Ref);
            if((user1 == null) || (user2 == null))
                return;
            await _repository.AddFriend(friend);
            string connectionId = _userMenager.LiveFriends.GetValueOrDefault(friend.User2Ref);
            if(connectionId == null)
                return;
            await Clients.Client(connectionId).SendAsync("RequestAccepted", friend.User1Ref);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if(!_userMenager.LiveFriends.ContainsKey(Context.UserIdentifier))
                _userMenager.LiveFriends.Add(Context.UserIdentifier, Context.ConnectionId);
            else
                return  null;
            List<string> liveFriends = new List<string>();
            User user = await _repository.GetUserByUsername(Context.UserIdentifier);
            ICollection<Friend> friends = user.Friends;
            foreach(var el in _userMenager.LiveFriends)
            {
                foreach(Friend f in friends)
                    if(f.User2Ref == el.Key)
                    {
                        liveFriends.Add(el.Key);
                        await Clients.Client(el.Value).SendAsync("UserConnected", Context.UserIdentifier);
                        break;
                    }
            }
            await Clients.Caller.SendAsync("GetLiveFriends", new JsonResult(new {friendsList = liveFriends}));
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            if(_userMenager.LiveFriends.ContainsKey(Context.UserIdentifier))
                _userMenager.LiveFriends.Remove(Context.UserIdentifier);
            await Clients.All.SendAsync("UserDisconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}