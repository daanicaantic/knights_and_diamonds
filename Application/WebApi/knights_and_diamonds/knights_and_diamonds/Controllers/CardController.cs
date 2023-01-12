
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
	public class CardController : ControllerBase
	{
		/*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }*/
		private readonly KnightsAndDiamondsContext context;
		public ICardService _cardService { get; set; }
		public CardController(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_cardService = new CardService(this.context);
		}

		[Route("AddMonsterCard")]
		[HttpPost]
		public async Task<IActionResult> AddMonsterCard([FromBody] Card card)
		{
			try
			{
				this._cardService.AddCard(card);
				return Ok(card);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		[Route("GetCard")]
		[HttpGet]
		public async Task<IActionResult> GetCard(int id)
		{
			try
			{
				if(id > 0) {
					return new JsonResult(this._cardService.GetCard(id));
				}
				else
				{
					return BadRequest("ID must be bigger than 0");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		[Route("GetCardByName")]
		[HttpGet]
		public async Task<IActionResult> GetCardByName(string name)
		{
			try
			{
				return new JsonResult(this._cardService.FindCardByName(name).FirstOrDefault());
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		[Route("DeleteCard")]
		[HttpDelete]
		public async Task<IActionResult> DeleteCard(int id)
		{
			try
			{
				Card c=this._cardService.GetCard(id);
				if (c != null) { 
					this._cardService.RemoveCard(c);
					return Ok();
				}
				else
				{
					return NotFound("Card with this ID dosent exist");
				}
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		[Route("UpdateCard")]
		[HttpPut]
		public async Task<IActionResult> UpdateCard([FromBody] Card card)
		{
			try
			{
				this._cardService.UpdateCard(card);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

	}
}