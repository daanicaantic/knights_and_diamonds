using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TypesController : ControllerBase
	{
		private readonly KnightsAndDiamondsContext context;

		public TypesController(KnightsAndDiamondsContext context)
		{
			this.context = context;
		}

		[Route("AddMonsterType")]
		[HttpPost]
		public async Task<IActionResult> AddMonsterType([FromBody] MonsterType type)
		{
			try
			{
				this.context.MonsterTypes.Add(type);
				await this.context.SaveChangesAsync();

				return Ok(type);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("RemoveMonsterType")]
		[HttpPost]
		public async Task<IActionResult> RemoveMonsterType(int id)
		{
			try
			{
				var mt=await this.context.MonsterTypes.FindAsync(id);
				this.context.Remove(mt);
				await this.context.SaveChangesAsync();

				return Ok(mt);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("GetMonsterTypes")]
		[HttpGet]
		public async Task<IActionResult> GetMonsterTypes()
		{
			try
			{
				var monsterTypes=await this.context.MonsterTypes.ToListAsync();
				return Ok(monsterTypes);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("GetCardTypes")]
		[HttpGet]
		public async Task<IActionResult> GetCardTypes()
		{
			try
			{
				var cardTypes = await this.context.CardTypes.ToListAsync();
				return Ok(cardTypes);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[Route("GetElementTypes")]
		[HttpGet]
		public async Task<IActionResult> GetElementTypes()
		{
			try
			{
				var elementTypes = await this.context.ElementTypes.ToListAsync();
				return Ok(elementTypes);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
	}

}
