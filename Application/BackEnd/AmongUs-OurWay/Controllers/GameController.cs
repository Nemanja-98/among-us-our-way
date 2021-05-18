using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AmongUs_OurWay.Models;
using AmongUs_OurWay.DataManagement;

namespace AmongUs_OurWay.Controllers
{
    [EnableCors("ServerPolicyV1")]
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private IGameRepository _repository;
        public GameController(Repository repo)
        {
            _repository = repo.GetGameRepository();
        }

        [HttpGet]
        [Route("getGame/{gameId}")]
        public async Task<ActionResult<Game>> GetGame(int gameId)
        {
            ActionResult<Game> result = await _repository.GetGame(gameId);
            if(result == null)
                return NotFound();
            return result;
        }

        [HttpPost]
        [Route("addAction")]
        public async Task<ActionResult> PostAction(PlayerAction action)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            ServerResponse retVal = await _repository.AddAction(action);
            if(retVal == ServerResponse.NotFound)
                return NotFound();
            return Ok();
        }

        [HttpPost]
        [Route("addPlayer")]
        public async Task<ActionResult> PostPlayer(GameHistory gameHistory)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            ServerResponse retVal = await _repository.AddPlayer(gameHistory);
            if(retVal == ServerResponse.NotFound)
                return NotFound();
            return Ok();
        }

        [HttpPost]
        [Route("addGame")]
        public async Task<ActionResult> PostGame(Game game)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            await _repository.AddGame(game);
            return Ok();
        }
    }
}
