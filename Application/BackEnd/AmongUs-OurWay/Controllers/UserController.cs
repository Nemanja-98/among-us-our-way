using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AmongUs_OurWay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AmongUs_OurWay.Controllers
{
    [Authorize]
    [EnableCors("ServerPolicyV1")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private AmongUsContext dbContext;

        public UserController(AmongUsContext db)
        {
            dbContext = db;
        }

        [HttpGet]
        [Route("getUsers")]
        public ActionResult<List<User>> GetUsers(string username)
        {
            return dbContext.Users.ToList();
        }

        [HttpGet]
        [Route("getUser/{username}")]
        public ActionResult<User> GetUser(string username)
        {
            User user = dbContext.Users.Find(username);
            if(user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Route("addUser")]
        public ActionResult PostUser(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addGame")]
        public ActionResult PostGame(GameHistory game)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User user = dbContext.Users.Find(game.UserId);
            if(user == null)
                return NotFound("User not found");
            user.Games.Add(game);
            dbContext.GameHistorys.Add(game);
            dbContext.SaveChanges();
            return Ok();
        }
        
        [HttpPost]
        [Route("addFriend")]
        public ActionResult PostFriend(Friend friend)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User user1 = dbContext.Users.Find(friend.User1Ref);
            User user2 = dbContext.Users.Find(friend.User2Ref);
            if((user1 == null) || (user2 == null))
                return NotFound("Users not found");
            Friend reverseFriend = new Friend{
                User1Ref = friend.User2Ref,
                User2Ref = friend.User1Ref};
            dbContext.Friends.Add(friend);
            dbContext.Friends.Add(reverseFriend);
            user1.Friends.Add(friend);
            user2.Friends.Add(reverseFriend);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addRequest")]
        public ActionResult PostRequest(PendingRequest pendingRequest)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User userSent = dbContext.Users.Find(pendingRequest.UserSentRef);
            User userRecieved = dbContext.Users.Find(pendingRequest.UserReceivedRef);
            if((userSent == null) || (userRecieved == null))
                return NotFound("Users not found");
            userSent.SentRequests.Add(pendingRequest);
            userRecieved.PendingRequests.Add(pendingRequest);
            dbContext.PendingRequests.Add(pendingRequest);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addAction")]
        public ActionResult PostAction(PlayerAction action)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User user = dbContext.Users.Find(action.UserId);
            if(user == null)
                return NotFound("User not found");
            user.Actions.Add(action);
            dbContext.PlayerActions.Add(action);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("updateUser")]
        public ActionResult PutUser(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User swap = dbContext.Users.Find(user.Username);
            swap.Password = user.Password;
            swap.GamesPlayed = user.GamesPlayed;
            swap.CrewmateGames = user.CrewmateGames;
            swap.ImpostorGames = user.ImpostorGames;
            swap.CrewmateWonGames = user.CrewmateWonGames;
            swap.ImpostorWonGames = user.ImpostorWonGames;
            swap.TasksCompleted = user.TasksCompleted;
            swap.AllTasksCompleted = user.AllTasksCompleted;
            swap.Kills = user.Kills;
            dbContext.SaveChanges();
            return Ok();
        }
/////////////////////////////////////////////////////////////////////////Login
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult CreateToken([FromBody] LoginModel login)
        {
            IActionResult response = NotFound();
            var user= Authenticate(login);
            if (user!=null)
            {
                var tokenStr = BuildToken(user);
                response=Ok(new {token = tokenStr});
            }

            return response;
        }

        private string BuildToken(User u)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, u.Username)
                };
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AmongUsAwesomeSeacretKey"));
            var creds= new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token=new JwtSecurityToken("https://localhost:5001/", "http://localhost:8080/", 
            claims ,expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(LoginModel login)
        {
            User user;
            if((user = dbContext.Users.Find(login.Username)) == null)
                return null;
            if(user.Password == login.Password)
                return user;
            return null;
        }
    }
}