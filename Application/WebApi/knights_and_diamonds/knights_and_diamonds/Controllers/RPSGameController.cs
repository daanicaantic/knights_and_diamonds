using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.DesignPatterns;
using static System.Text.Json.JsonSerializer;


namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RPSGameController : ControllerBase
	{
		/*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
        */
		private readonly KnightsAndDiamondsContext context;
		public IRPSGamaeService _pregameservice { get; set; }
		public IUserService _userService { get; set; }

		public RPSGameController(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_pregameservice = new RPSGameService(this.context);
			_userService = new UserService(this.context);

		}
		[Route("NewLobby")]
		[HttpPost]
		public async Task<IActionResult> NewLobby(int userID,int challengedUserID)
		{
			try
			{
				if (userID == challengedUserID)
				{
					return BadRequest("Users IDs are the same,you cannot play against yourself!!");
				}

				var user = await this._userService.GetUserByID(userID);
				var challengedUser = await this._userService.GetUserByID(challengedUserID);
				if (user != null && challengedUser!=null)
				{
					this._pregameservice.NewLobby(user,challengedUser);
					return Ok();
				}
				else 
				{
					return NotFound("User is not found");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("StartGame")]
		[HttpPost]
		public async Task<IActionResult> StartGame(int lobbyID)
		{
			try
			{
				await this._pregameservice.StartGame(lobbyID);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetGames")]
		[HttpGet]
		public async Task<IActionResult> GetGames()
		{
			try
			{

				var games = await this._pregameservice.GetGames() ;
				return new JsonResult(games);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("GetLobies")]
		[HttpGet]
		public async Task<IActionResult> GetLobies(int userID)
		{
			try
			{

				var games =await this._pregameservice.LobbiesPerUser(userID);
				return new JsonResult(games);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}
}
