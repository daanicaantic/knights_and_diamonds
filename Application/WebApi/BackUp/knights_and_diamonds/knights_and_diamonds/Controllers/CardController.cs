
using DAL.DataContext;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CardController : ControllerBase
	{

		private IRepository<MonsterCard> context { get; set; }
		public CardController(IRepository<MonsterCard> repository)
		{
			context = repository;
		}

		[Route("AddMonsterCard")]
		[HttpPost]
		public async Task<IActionResult> AddMonsterCard([FromBody] MonsterCard card)
		{
			try
			{
				this.context.Add(card);
				return Ok(card);
			}
			catch (Exception e)
			{
				return BadRequest("lose");
			}
		}
		//[Route("AddSpellTrapCard")]
		//[HttpPost]
		//public async Task<ActionResult> AddSpellTrapCard([FromBody] SpellTrapCard card)
		//{
		//	try
		//	{
		//		context.Cards.Add(card);
		//		await context.SaveChangesAsync();
		//		return Ok(card);
		//	}
		//	catch (Exception e)
		//	{
		//		return BadRequest(e.Message);
		//	}
		//}
		//[Route("GetMonsterCards")]
		//[HttpGet]
		//public async Task<ActionResult> GetMonsterCards()
		//{
		//	return Ok(await context.Cards.Select(p =>
		//	new
		//	{
		//		id = p.ID,
		//		card_name = p.CardName,
		//		img_path=p.ImgPath,
		//		effect=p.Effect
		//	}
		//	).ToListAsync());
		//}

	}
}