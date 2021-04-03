using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AmongUs_OurWay.Hubs
{
    public class FriendHub : Hub
    {
        private AmongUsContext dbContext;
        private LiveUsersMenager userMenager;
        public FriendHub(AmongUsContext db, LiveUsersMenager mng)
        {
            dbContext = db;
            userMenager = mng;
        }

        public async Task SendFriendRequest(string userSentRef, string userReceivedRef)
        {
            if(userSentRef == null || userReceivedRef == null)
                return;
            User userSent = dbContext.Users.Find(userSentRef);
            User userRecieved = dbContext.Users.Find(userReceivedRef);
            if((userSent == null) || (userRecieved == null))
                return;
            PendingRequest pendingRequest = new PendingRequest{
                UserSentRef = userSentRef,
                UserReceivedRef = userReceivedRef
            };
            dbContext.PendingRequests.Add(pendingRequest);
            dbContext.SaveChanges();
            string connectionId = userMenager.LiveFriends.GetValueOrDefault(userReceivedRef);
            if(connectionId == null)
                return;
            await Clients.Client(connectionId).SendAsync("RequestReceived", userSent.Username);
        }

        public async Task AcceptFriendRequest(Friend friend)
        {
            if(friend.User1Ref == null || friend.User2Ref == null)
                return;
            User user1 = dbContext.Users.Find(friend.User1Ref);
            User user2 = dbContext.Users.Find(friend.User2Ref);
            if((user1 == null) || (user2 == null))
                return;
            Friend reverseFriend = new Friend{
                User1Ref = friend.User2Ref,
                User2Ref = friend.User1Ref};
            dbContext.Friends.Add(friend);
            dbContext.Friends.Add(reverseFriend);
            dbContext.SaveChanges();
            string connectionId = userMenager.LiveFriends.GetValueOrDefault(friend.User2Ref);
            if(connectionId == null)
                return;
            await Clients.Client(connectionId).SendAsync("RequestAccepted", friend.User1Ref);
        }

        public override async Task<Task> OnConnectedAsync()
        {
            if(!userMenager.LiveFriends.ContainsKey(Context.UserIdentifier))
                userMenager.LiveFriends.Add(Context.UserIdentifier, Context.ConnectionId);
            else
                return  null;
            List<string> liveFriends = new List<string>();
            User user = dbContext.Users.Find(Context.UserIdentifier);
            ICollection<Friend> friends = user.Friends;
            foreach(var el in userMenager.LiveFriends)
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
            if(userMenager.LiveFriends.ContainsKey(Context.UserIdentifier))
                userMenager.LiveFriends.Remove(Context.UserIdentifier);
            await Clients.All.SendAsync("UserDisconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}