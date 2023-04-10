using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using DAL.DesignPatterns;
using static System.Text.Json.JsonSerializer;
using System.Runtime.Intrinsics.X86;
using DAL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
		private readonly KnightsAndDiamondsContext _context;
		public IRPSGameService _pregameservice { get; set; }
		public IUserService _userService { get; set; }

		public RPSGameController(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_pregameservice = new RPSGameService(this._context);
			_userService = new UserService(this._context);
		}

		[Route("NewLobby")]
		[HttpPost]
		public async Task<IActionResult> NewLobby(int userID, int challengedUserID)
		{
			try
			{
				if (userID == challengedUserID)
				{
					return BadRequest("Users IDs are the same, you cannot play against yourself!!");
				}

				var user = await this._userService.GetUserByID(userID);
				var challengedUser = await this._userService.GetUserByID(challengedUserID);

                if (user == null || challengedUser == null)
                {
					return NotFound("User does not exists.");
                }

                var player1 = new OnlineUserDto(user.ID, user.Name, user.SurName, user.UserName);
				var player2 = new OnlineUserDto(challengedUser.ID, challengedUser.Name, challengedUser.SurName, challengedUser.UserName);

				if (user != null && challengedUser!=null)
				{
					var lobbyID = await this._pregameservice.NewLobby(player1, player2);
					return Ok(lobbyID);
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

		[Route("StartGame/{lobbyID}")]
		[HttpPost]
		public async Task<IActionResult> StartGame(int lobbyID)
		{
			try
			{
				var game = await this._pregameservice.StartGame(lobbyID);
				return Ok(game);
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
				var games = await this._pregameservice.GetGames();
				return new JsonResult(games);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("GetRPSGame/{gameID}/{userID}")]
		[HttpGet]
		public async Task<IActionResult> GetGame(int gameID,int userID)
		{
			try
			{
				var game = await this._pregameservice.GetGame(gameID,userID);
				return Ok(game);
				
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[Route("GetPlayerMove/{playerID}")]
		[HttpGet]
		public async Task<IActionResult> GetPlayerMove(int playerID)
		{
			try
			{
				var move=await this._pregameservice.GetPlayersMove(playerID);
				return Ok(move);

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetLobies")]
		[HttpGet]
		public async Task<IActionResult> GetLobies(int userID)
		{
			try
			{
				var user = await this._userService.GetUserByID(userID);
				if (user != null)
				{
					var games = this._pregameservice.LobbiesPerUser(userID);
					return new JsonResult(games);
				}
				else 
				{
					return BadRequest("User is not found");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("DenyGame/{lobbyID}")]
		[HttpDelete]
		public async Task<IActionResult> DenyGame(int lobbyID)
		{
            try
            {
				var game = await this._pregameservice.DenyGame(lobbyID);
				return Ok(game);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Redirect")]
        [HttpGet]
        public async Task<IActionResult> Redirect(int gameID)
        {
            try
            {
                var userIDs = await this._pregameservice.RedirectToGame(gameID);
                return Ok(userIDs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

		[Route("PlayMove/{playerID}/{moveName}")]
		[HttpPut]
		public async Task<IActionResult> PlayMove(int playerID, string moveName)
		{
            try
            {
                await this._pregameservice.PlayMove(playerID, moveName);
				return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("CheckRPSWinner")]
        [HttpGet]
        public async Task<IActionResult> CheckRPSWinner(int gameID)
        {
            try
            {
				var winner = await this._pregameservice.CheckRPSWinner(gameID);
                return Ok(winner);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

		[Route("GetPlayer/{gameID}/{userID}")]
		[HttpGet]
		public async Task<IActionResult> GetPlayer(int gameID, int userID)
		{
			try
			{
				var player = await this._pregameservice.GetPlayer(gameID, userID);
				return Ok(player);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("RemoveUserFromUsersInGame/{userID}")]
		[HttpDelete]
		public async Task<IActionResult> RemoveUserFromUsersInGame(int userID)
		{
			try
			{
				await this._pregameservice.RemoveUserFromUsersInGame(userID);
				return Ok();
			}
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
