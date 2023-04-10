using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
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
        private readonly KnightsAndDiamondsContext context;
        public IGameService _gameservice { get; set; }
        public IPlayerService _playerservice { get; set; }
		public MyHub _hubContext { get; set; }
		private readonly IConfiguration _config;

		public GameController(KnightsAndDiamondsContext context, IConfiguration config)
		{
			this.context = context;
			this._config = config;
			this._gameservice = new GameService(this.context);
			this._playerservice = new PlayerService(this.context);
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

		[Route("GetField/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetField(int playerID)
		{
			try
			{
	
				var field = await this._gameservice.GetPlayersField(playerID);
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

		[Route("GetHand/{gameID}/{playerID}")]
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
		}
	}
}
