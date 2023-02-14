
using DAL.DataContext;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.Services.Contracts;
using BLL.Services;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectionController : ControllerBase
	{
		/*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
		*/
		private readonly KnightsAndDiamondsContext context;
		public IConnectionService _connectionService { get; set; }
		public ConnectionController(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_connectionService = new ConnectionService(this.context);
		}


		[HttpPost]
		[Route("AddOnlineUser")]
		public async Task<IActionResult> AddOnlineUser(int userID, string connID)
		{
			try
			{
				this._connectionService.AddOnlineUser(userID, connID);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("GetConnectionByID")]
		public async Task<IActionResult> GetConnectionByID(int id)
		{
			var c = await this._connectionService.GetConnectionByUser(id);
			return new JsonResult(c);

		}
		[HttpGet]
		[Route("GetOnlineUsers")]
		public async Task<IActionResult> GetOnlineUsers()
		{
			try
			{
				var c = await this._connectionService.GetOnlineUsers();

				return new JsonResult(c);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}