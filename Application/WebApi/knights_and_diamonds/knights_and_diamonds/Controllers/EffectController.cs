using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using DAL.DesignPatterns.Factory.Contract;
using DAL.DesignPatterns.Factory;
using Microsoft.AspNetCore.Mvc;
using BLL.Strategy;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EffectController : ControllerBase
	{
		/*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
		*/
		private readonly KnightsAndDiamondsContext _context;
		public IEffectService _effectService { get; set; }
		public EffectController(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_effectService = new EffectService(context);
		}

		[Route("GetAreaOfClicking/{effectTypeID}")]
		[HttpGet]
		public async Task<IActionResult> GetAreaOfClicking(int effectTypeID)
		{
			try
			{
				var area = await this._effectService.GetAreaOfClickingAfterPlayCard(effectTypeID);
				return Ok(area);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

	}
}
