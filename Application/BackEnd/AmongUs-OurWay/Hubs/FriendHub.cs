using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AmongUs_OurWay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AmongUs_OurWay.Hubs
{
    [Authorize]
    public class FriendHub : Hub
    {
        private AmongUsContext dbContext;
        private LiveUsersMenager userMenager;
        public FriendHub(AmongUsContext db, LiveUsersMenager mng)
        {
            dbContext = db;
            userMenager = mng;
        }

        public Task SendFriendRequest(PendingRequest pendingRequest)
        {
            if(dbContext.Users.Find(pendingRequest.UserSentRef) == null || dbContext.Users.Find(pendingRequest.UserReceivedRef) == null)
                return null;
            if(pendingRequest.UserSentRef == null || pendingRequest.UserReceivedRef == null)
                return null;
            User userSent = dbContext.Users.Find(pendingRequest.UserSentRef);
            User userRecieved = dbContext.Users.Find(pendingRequest.UserReceivedRef);
            if((userSent == null) || (userRecieved == null))
                return null;
            userSent.SentRequests.Add(pendingRequest);
            userRecieved.PendingRequests.Add(pendingRequest);
            dbContext.PendingRequests.Add(pendingRequest);
            dbContext.SaveChanges();
            string connectionId = userMenager.LiveUsers.GetValueOrDefault(pendingRequest.UserReceivedRef);
            if(connectionId == null)
                return null;
            return Clients.Client(connectionId).SendAsync("RequestReceived", userSent.Username);
        }

        public Task AcceptFriendRequest(Friend friend)
        {
            if(friend.User1Ref == null || friend.User2Ref == null)
                return null;
            User user1 = dbContext.Users.Find(friend.User1Ref);
            User user2 = dbContext.Users.Find(friend.User2Ref);
            if((user1 == null) || (user2 == null))
                return null;
            Friend reverseFriend = new Friend{
                User1Ref = friend.User2Ref,
                User2Ref = friend.User1Ref};
            dbContext.Friends.Add(friend);
            dbContext.Friends.Add(reverseFriend);
            dbContext.SaveChanges();
            string connectionId = userMenager.LiveUsers.GetValueOrDefault(friend.User2Ref);
            if(connectionId == null)
                return null;
            return Clients.Client(connectionId).SendAsync("RequestAccepted", friend.User1Ref);
        }

        public override Task OnConnectedAsync()
        {
            List<string> liveFriends = new List<string>();
            Console.WriteLine("Usao " + Context.UserIdentifier);
            User user = dbContext.Users.Find(Context.UserIdentifier);
            ICollection<Friend> friends = user.Friends;
            foreach(var el in userMenager.LiveUsers)
            {
                foreach(Friend f in friends)
                    if(f.User2Ref == el.Key)
                    {
                        liveFriends.Add(el.Key);
                        Clients.Client(el.Value).SendAsync("UserConnected", Context.UserIdentifier);
                        break;
                    }
            }
            Clients.Caller.SendAsync("GetLiveFriends", new JsonResult(new {friendsList = liveFriends}));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.SendAsync("UserDisconnected", Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}