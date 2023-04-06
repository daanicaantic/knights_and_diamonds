
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

		public IEffectFactory _descriptionFactory { get; set; }
		public IFactory _factory { get; set; }
		public CardController(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_cardService = new CardService(this.context);
			_descriptionFactory = new ConcreteEffectFactory();
			_effService = new EffectService(this.context);
		}


		[Route("AddCard")]
		[HttpPost]
		public async Task<IActionResult> AddCard(CardDTO card)
		{
			try
			{
				await this._cardService.AddCard(card);
				return Ok();
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
					await this._cardService.RemoveCard(c);
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
					await this._cardService.UpdateCard(card);
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
		public async Task<IActionResult> AddCardType([FromBody] CardType type)
		{
			try
			{
				this.context.CardTypes.Add(type);
				await this.context.SaveChangesAsync();

				return Ok(type);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

        [Route("GetAllCards")]
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            try
            {
                var cards=await this._cardService.GetAllCards();

                return Ok(cards);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}