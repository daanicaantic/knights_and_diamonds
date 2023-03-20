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
    public class DeckController : ControllerBase
    {
        /*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
        */
        private readonly KnightsAndDiamondsContext context;
        public IDeckService _deckService { get; set; }
        public DeckController(KnightsAndDiamondsContext context)
        {
            this.context = context;
            _deckService = new DeckService(this.context);
        }

        [Route("AddDeck")]
        [HttpPost]
        public async Task<IActionResult> AddDeck([FromBody] Deck deck)
        {
            try
            {
                var d=await this._deckService.AddDeck(deck);
                return Ok(d);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("AddCardToDeck")]
        [HttpPost]
        public async Task<IActionResult> AddCardToDeck(int cardID,int deckID)
        {
            try
            {
                await this._deckService.AddCardToDeck(cardID, deckID);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

		[Route("GetDeck")]
        [HttpGet]
        public async Task<IActionResult> GetDeck(int deckID, int userID)
        {
            try
            {
                if (userID <= 0)
                {
                    return BadRequest("ID must be bigger than 0");
                }
                var c = await this._deckService.GetCards(deckID, userID);
                return new JsonResult(c);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
		}
    }
}