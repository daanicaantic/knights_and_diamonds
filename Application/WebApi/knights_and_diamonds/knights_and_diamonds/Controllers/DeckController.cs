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
                this._deckService.AddDeck(deck);
                return Ok(deck);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("AddCardToDeck")]
        [HttpPut]
        public async Task<IActionResult> AddCardToDeck(int cardID,int deckID)
        {
            try
            {
                this._deckService.AddCardToDeck(cardID, deckID);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [Route("GetDeck")]
        [HttpGet]
        public async Task<IActionResult> GetDeck(int id)
        {
            try
            {
                if (id > 0)
                {
                    return new JsonResult(this._deckService.GetDeck(id));
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
    }
}