using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AmongUs_OurWay.Models;

namespace AmongUs_OurWay.Controllers
{
    [EnableCors("ServerPolicyV1")]
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private AmongUsContext dbContext;
        public GameController(AmongUsContext db)
        {
            dbContext = db;
        }

        [HttpGet]
        [Route("getGame/{gameId}")]
        public ActionResult<Game> GetGame(int gameId)
        {
            Game game = dbContext.Games.Find(gameId);
            if(game == null)
                return NotFound("Game not found");
            return game;
        }

        [HttpPost]
        [Route("addAction")]
        public ActionResult PostAction(PlayerAction action)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            Game game = dbContext.Games.Find(action.GameId);
            if(game == null)
                return NotFound("Game not found");
            game.Actions.Add(action);
            dbContext.PlayerActions.Add(action);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addPlayer")]
        public ActionResult PostPlayer(GameHistory gameHistory)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            Game game = dbContext.Games.Find(gameHistory.GameId);
            if(game == null)
                return NotFound("Game not found");
            game.Players.Add(gameHistory);
            dbContext.GameHistorys.Add(gameHistory);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addGame")]
        public ActionResult PostGame(Game game)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid input values");
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
