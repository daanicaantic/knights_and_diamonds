using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.HubConfig;
using System.Runtime.CompilerServices;

namespace knights_and_diamonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
		struct Field
		{
			public FieldDTO PlayerField { get; set; }
			public EnemiesFieldDTO EnemiesField { get; set; }
		}
		private readonly KnightsAndDiamondsContext _context;
        public IGameService _gameservice { get; set; }
        public IPlayerService _playerservice { get; set; }
		public ITurnService _turnService { get; set; }

		public MyHub _hubContext { get; set; }
		private readonly IConfiguration _config;

		public GameController(KnightsAndDiamondsContext context, IConfiguration config)
		{
			this._context = context;
			this._config = config;
			this._gameservice = new GameService(this._context);
			this._playerservice = new PlayerService(this._context);
			this._turnService = new TurnService(this._context);

			this._hubContext = new MyHub(context,_config);
		}
		/*[Route("StartGame/{player1ID}/{player2ID}")]
        [HttpPost]
        public async Task<IActionResult> StartGame(int player1ID, int player2ID)
        {
            try
            {
                var game = await this._gameservice.StartGame(player1ID, player2ID);
                return Ok(game);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

		[Route("GetGame/{gameID}/{userID}")]
		[HttpGet]
		public async Task<IActionResult> GetGame(int gameID, int userID)
		{
			try
			{
				var game = await this._gameservice.GetGame(gameID, userID);
				return Ok(game);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetStartingField/{playerID}/{enemiesPlayerID}")]
		[HttpGet]
		public async Task<IActionResult> GetStartingField(int playerID,int enemiesPlayerID)
		{
			try
			{
				var field = new Field();
				
				var playersField = await this._gameservice.GetPlayersField(playerID);
				var f = await this._gameservice.GetPlayersField(enemiesPlayerID);
				var enemiesField = this._gameservice.GetEneiesField(f);

				field.PlayerField = playersField;
				field.EnemiesField = enemiesField;

				return Ok(field);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetEnemiesField/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetEnemiesField(int playerID)
		{
			try
			{
				var field = await this._gameservice.GetPlayersField(playerID);
				var enemiesField = this._gameservice.GetEneiesField(field);
				return Ok(enemiesField);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

	/*	[Route("GetGamePhase/{gameID}")]
		[HttpGet]
		public async Task<IActionResult> GetGamePhase(int gameID)
		{
			try
			{
				var gp=await this._turnService.GetTurnPhase(gameID);
				return Ok(gp);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}*/
		[Route("GetTurnInfo/{gameID}/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetTurnInfo(int gameID,int playerID)
		{
			try
			{
				var turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				return Ok(turnInfo);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
/*		[Route("NewTurn/{gameID}")]
		[HttpPost]
		public async Task<IActionResult> NewTurn(int gameID)
		{
			try
			{
				var turnInfo = await this._turnService.NewTurn(gameID);
				return Ok(turnInfo);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}*/

		[Route("GetGrave/{gameID}")]
		[HttpGet]
		public async Task<IActionResult> GetGrave(int gameID)
		{
			try
			{
				var grave = await this._gameservice.GetGamesGrave(gameID);
				return Ok(grave);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/*	[Route("GetHand/{gameID}/{playerID}")]
			[HttpGet]
			public async Task<IActionResult> GetHand(int gameID,int playerID)
			{
				try
				{
					await this._hubContext.StartingDrawing(gameID, playerID);
					return Ok("bravo");

				}
				catch (Exception e)
				{
					return BadRequest(e.Message);
				}
			}*/

		[Route("GetHand/{gameID}")]
		[HttpGet]
		public async Task<IActionResult> GetGameGroup(int gameID)
		{
			try
			{
				var gg=await this._gameservice.GameGroup(gameID);
				return Ok(gg);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
