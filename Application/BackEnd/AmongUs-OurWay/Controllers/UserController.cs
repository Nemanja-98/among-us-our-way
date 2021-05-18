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
using AmongUs_OurWay.Hubs;
using AmongUs_OurWay.DataManagement;

namespace AmongUs_OurWay.Controllers
{
    [Authorize]
    [EnableCors("ServerPolicyV1")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;
        private LiveGamesMenager _gameMenager;
        private Random rand = new Random();

        public UserController(Repository repo, LiveGamesMenager mng)
        {
            _repository = repo.GetUserRepository();
            _gameMenager = mng;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getUsers")]
        public async Task<List<User>> GetUsers()
        {
            return await _repository.UserList();
        }

        [HttpGet]
        [Route("search/{substr}")]
        public async Task<List<User>> getSearch(string substr)
        {
            return await _repository.Search(substr);
        }

        [HttpGet]
        [Route("getUser/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            ActionResult<User> result = await _repository.GetUserByUsername(username);
            if(result == null)
                result = NotFound();
            return result;
        }

        [HttpGet]
        [Route("getUserByToken")]
        public async Task<ActionResult<User>> GetUserByToken()
        {
            string username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(username == null)
                return BadRequest();
            User user = await _repository.GetUserByUsername(username);
            if(user == null)
                return NotFound();
            return user;
        }

        [HttpGet]
        [Route("userMessages/{userSentId}/{userReceivedId}")]
        public async Task<ActionResult> GetMessages(string userSentId, string userReceivedId)
        {
            User userSent = await _repository.GetUserByUsername(userSentId);
            User userReceived = await _repository.GetUserByUsername(userReceivedId);
            if(userSent == null || userReceived == null)
                return NotFound();
            string callerUsername =  User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(callerUsername == null || callerUsername != userSent.Username)
                return Unauthorized();
            return await _repository.GetMessages(userSent, userReceived);
        }

        [HttpGet]
        [Route("generateCode")]
        public ActionResult GetCode()
        {
            char[] code = new char[Constants._codeLength];
            string result = "";
            do
            {
                for(int i = 0 ; i < Constants._codeLength ; i++)
                    code[i] = Constants._codeChars[rand.Next(Constants._codeChars.Length)];
                result =  new String(code);
            }while(_gameMenager.LiveGames.Contains(result));
            _gameMenager.LiveGames.Add(result);
            return new JsonResult(new {code = result});
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("addUser")]
        public async Task<ActionResult> PostUser(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            if(_repository.GetUserByUsername(user.Username) != null)
                return Conflict();
            await _repository.SaveUser(user);
            return Ok();
        }

        [HttpPost]
        [Route("addGame")]
        public async Task<ActionResult> PostGame(GameHistory game)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            ServerResponse retVal = await _repository.AddGame(game, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if(retVal == ServerResponse.NotFound)
                return NotFound();
            if(retVal == ServerResponse.Unauthorized)
                return Unauthorized();
            return Ok();
        }
        
        [HttpPost]
        [Route("addFriend")]
        public async Task<ActionResult> PostFriend(Friend friend)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User user1 = await _repository.GetUserByUsername(friend.User1Ref);
            User user2 = await _repository.GetUserByUsername(friend.User2Ref);
            if((user1 == null) || (user2 == null))
                return NotFound("Users not found");
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != user1.Username)
                return Unauthorized();
            await _repository.AddFriend(friend);
            return Ok();
        }

        [HttpPost]
        [Route("addRequest")]
        public async Task<ActionResult> PostRequest(PendingRequest pendingRequest)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User userSent = await _repository.GetUserByUsername(pendingRequest.UserSentRef);
            User userRecieved = await _repository.GetUserByUsername(pendingRequest.UserReceivedRef);
            if((userSent == null) || (userRecieved == null))
                return NotFound("Users not found");
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != userSent.Username)
                return Unauthorized();
            await _repository.AddRequest(pendingRequest);
            return Ok();
        }

        [HttpPost]
        [Route("addAction")]
        public async Task<ActionResult> PostAction(PlayerAction action)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User user = await _repository.GetUserByUsername(action.UserId);
            if(user == null)
                return NotFound("User not found");
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != user.Username)
                return Unauthorized();
            await _repository.AddAction(action);
            return Ok();
        }

        [HttpPut]
        [Route("updateUser")]
        public async Task<ActionResult> PutUser(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            User swap = await _repository.GetUserByUsername(user.Username);
            if(swap == null)
                return NotFound("User not found");
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != swap.Username)
                return Unauthorized();
            await _repository.UpdateUser(swap);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteRequest/{requestId}")]
        public async Task<ActionResult> DeleteRequest(string requestId)
        {
            
            ServerResponse retVal = await _repository.DeleteRequest(requestId, User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if(retVal == ServerResponse.NotFound)
                return NotFound();
            if(retVal == ServerResponse.Unauthorized)
                return Unauthorized();
            return Ok();
        }
/////////////////////////////////////////////////////////////////////////Login
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel login)
        {
            IActionResult response = NotFound();
            var user= await Authenticate(login);
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

            var token=new JwtSecurityToken("https://localhost:5001/", "http://localhost:4200/", 
            claims ,expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> Authenticate(LoginModel login)
        {
            User user;
            if((user = await _repository.GetUserByUsername(login.Username)) == null)
                return null;
            if(user.Password == login.Password)
                return user;
            return null;
        }
    }
}