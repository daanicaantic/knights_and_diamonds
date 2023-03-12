
using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace knights_and_diamonds.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CardController : ControllerBase
	{
		/*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
		*/
		private readonly KnightsAndDiamondsContext context;
		public ICardService _cardService { get; set; }
		public IEffectService _effService { get; set; }

		public IDescriptionFactory _descriptionFactory { get; set; }
		public IFactory _factory { get; set; }
		public CardController(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_cardService = new CardService(this.context);
			_descriptionFactory = new ConcreteDescriptionFactory();
			_effService = new EffectService(this.context);
		}

		[Route("AddCard")]
		[HttpPost]
		public async Task<IActionResult> AddCard([FromBody] CardDTO card)
		{
			try
			{
				if (card.CardTypeID == 3)
				{
					return BadRequest("There is some error");
				}
				var c = await this._cardService.AddCard(card);
				return Ok(c);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[Route("AddEffect")]
		[HttpPost]
		public async Task<IActionResult> AddEffect([FromBody] CardDTO card)
		{
			try
			{
				var e=await this._effService.AddEffect(card.EffectTypeID, card.NumOfCardsAffected, card.PointsAddedLost);
				return Ok(e);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("AddMonsterCard")]
		[HttpPost]
		public async Task<IActionResult> AddMonsterCard([FromBody] MonsterCard MonsterCard)
		{
			try
			{
				var c=await this._cardService.AddMonsterCard(MonsterCard);
				return Ok(c);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[Route("GetCard")]
		[HttpGet]
		public async Task<IActionResult> GetCard(int id)
		{
			try
			{
				if(id > 0) {
					var card=await this._cardService.GetCard(id);
					if (card != null)
					{
						return new JsonResult(card);
					}
					else
					{
						return NotFound("Card with this id doesnt exist");
					}
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
				var c = await this._cardService.GetCard(id);
				if (c != null)
				{
					this._cardService.RemoveCard(c);
					return Ok(c);
				}
				else
				{
					return NotFound("Card with this ID doesnt exist");
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
				if (card != null)
				{
					this._cardService.UpdateCard(card);
				}
				else 
				{
					return NotFound("This card doesnt exist");
				}
				return Ok(card);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		[Route("AddCardType")]
		[HttpPost]
		public async Task<IActionResult> AddCardType([FromBody] MonsterType type)
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
		[Route("GetD")]
		[HttpGet]
		public async Task<IActionResult> Getd(string type,string ct,int nuce,int points ) 
		{
			try
			{
				/*this._factory = this._descriptionFactory.FactoryMethod(type, ct, nuce, points);
				var description = await this._factory.GetDescription();*/
				return Ok();
				
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

	}
}