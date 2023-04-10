using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		private readonly KnightsAndDiamondsContext _context;
		public IGameService _gameservice { get; set; }
		public IPlayerService _playerservice { get; set; }
		public PlayerController(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._gameservice = new GameService(this._context);
			this._playerservice = new PlayerService(this._context);
		}

		[Route("GetPlayer/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetPlayer(int playerID)
		{
			try
			{
				var player = await this._playerservice.GetPlayer(playerID);
				return Ok(player);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[Route("SetStatus/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> SetStatus(int playerID)
		{
			try
			{
				var player = await this._playerservice.GetPlayer(playerID);
				await this._playerservice.SetGameStarted(player);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("Draw/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> Draw(int playerID)
		{
			try
			{
				return new JsonResult(await this._playerservice.Draw(playerID));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[Route("GetNumberOfCardsInDeck/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetNumberOfCardsInDeck(int playerID)
		{
			try
			{
				if (playerID <= 0)
				{
					return BadRequest("Error,wrong playerID");
				}
				var nuc = await this._playerservice.GetNumberOfCardsInDeck(playerID);
				return Ok(nuc);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetPlayersHand/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetPlayersHand(int playerID)
		{
			try
			{
				var card = await this._playerservice.GetPlayersHand(playerID);
				return Ok(card);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


	}
}
