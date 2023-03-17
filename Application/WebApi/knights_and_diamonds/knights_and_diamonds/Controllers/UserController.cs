using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.IdentityModel.Tokens.Jwt;
using DAL.Models;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace knights_and_diamonds.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        /*
				private readonly KnightsAndDiamondsContext context;
				public UnitOfWork unitOfWork { get; set; }
        */
        private readonly KnightsAndDiamondsContext context;
        public IUserService _userService { get; set; }

        public UserController(KnightsAndDiamondsContext context)
        {
            this.context = context;
            _userService = new UserService(this.context);
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
        {
            try
            {
                await this._userService.AddUser(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                if (id > 0)
                {
                    var user = await this._userService.GetUserByID(id);
                    if (user != null)
                    {
                        return new JsonResult(user);
                    }
                    else
                    {
                        return NotFound("User dosent exist");
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

        [Route("SetMainDeck")]
        [HttpPut]
        public async Task<IActionResult> SetMainDeck(int userID,int deckID)
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
                var user=await this._userService.SetMainDeckID(userID, deckID);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
