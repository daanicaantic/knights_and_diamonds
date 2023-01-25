using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;

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
		[Route("NewGame")]
		[HttpPost]
		public async Task<IActionResult> NewGame(int userid)
		{
			try
			{

				var user = await this._userService.GetUserByID(userid);
				if (user != null)
				{
					var newgame=this._pregameservice.NewGame(user);
					if (newgame) 
					{ 
						return Ok();
					}
					else 
					{
						return BadRequest("User is already in game");
					}
				}
				else 
				{
					return NotFound("user is not found");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}
}
