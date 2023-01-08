
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CardController : ControllerBase
	{

		private KnightsAndDiamondsContext context { get; set; }
		public CardController(KnightsAndDiamondsContext repository)
		{
			context = repository;
		}

		[Route("AddCard")]
		[HttpPost]
		public async Task<ActionResult> AddCard([FromBody] Card card,bool cardtype=true)
		{
			try 
			{
				context.Cards.Add(card);
				await context.SaveChangesAsync();
				return Ok(card);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
	}
}