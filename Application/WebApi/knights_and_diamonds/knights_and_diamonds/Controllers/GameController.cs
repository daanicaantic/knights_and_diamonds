using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knights_and_diamonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly KnightsAndDiamondsContext context;
        public IGameService _gameservice { get; set; }
        public GameController(KnightsAndDiamondsContext context)
        {
            this.context = context;
            this._gameservice = new GameService(this.context);
        }

        [Route("StartGame/{player1ID}/{player2ID}")]
        [HttpPost]
        public async Task<IActionResult> StartGame(int player1ID, int player2ID)
        {
            try
            {
                var game = await this._gameservice.StartGame(player1ID, player2ID);
                return Ok(game);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> delete()
        {
            try
            {
              
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
