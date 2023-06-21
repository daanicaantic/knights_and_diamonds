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
        private readonly KnightsAndDiamondsContext _context;
        public IDeckService _deckService { get; set; }
        public DeckController(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _deckService = new DeckService(this._context);
        }

        [Route("AddDeck/{userID}")]
        [HttpPost]
        public async Task<IActionResult> AddDeck(int userID)
        {
            try
            {
                var d = await this._deckService.AddDeck(userID);
                return Ok(d);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("SetMainDeck")]
        [HttpPut]
        public async Task<IActionResult> SetMainDeck(int userID, int deckID)
        {
            try
            {
                if (userID <= 0)
                {
                    return BadRequest("UserID must be bigger then 0");
                }
                if (deckID <= 0)
                {
                    return BadRequest("DeckID must be bigger then 0");
                }
                var user = await this._deckService.SetMainDeckID(userID, deckID);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Route("GetDeck/{userID}")]
        [HttpGet]
        public async Task<IActionResult> GetDeck(int userID)
        {
            try
            {
                if (userID <= 0)
                {
                    return BadRequest("ID must be bigger than 0");
                }
                var c = await this._deckService.GetCardsFromDeck(userID);
                return new JsonResult(c);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("AddCardToDeck/{cardID}/{deckID}")]
        [HttpPost]
        public async Task<IActionResult> AddCardToDeck(int cardID, int deckID)
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
        [Route("RemoveCardFromDeck/{cardID}/{deckID}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveCardFromDeck(int cardID, int deckID)
        {
            try
            {
                await this._deckService.RemoveCardFromDeck(cardID, deckID);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("CardCounter/{deckID}/{userID}")]
        [HttpGet]
        public async Task<IActionResult> CardCounter(int deckID, int userID)
        {
            try
            {
                if (userID <= 0 || deckID <= 0)
                {
                    return BadRequest("ID must be bigger than 0");
                }
                var count = await this._deckService.CardCounter(deckID, userID);
                return new JsonResult(count);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}