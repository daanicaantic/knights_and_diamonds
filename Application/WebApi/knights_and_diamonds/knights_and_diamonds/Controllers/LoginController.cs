using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.DTOs;
using DAL.Migrations;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using SignalR.HubConfig;

namespace knights_and_diamonds.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly KnightsAndDiamondsContext context;
        public IUserService _userService { get; set; }
		public ILoginService _loginService { get; set; }

		public IConnectionService _connetionService { get; set; }

		public LoginController(KnightsAndDiamondsContext context, IConfiguration config,IHubContext<MyHub> HubContext)
        {
            this.context = context;
            _userService = new UserService(this.context);
			_connetionService=new ConnectionService(this.context);
			_config = config;
			_loginService = new LoginService(this.context, this._config);

		}

		public static long CheckLoginToken(string token) 
        {
			var handler = new JwtSecurityTokenHandler();
			var jwtSecurityToken = handler.ReadJwtToken(token);
			var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
			var ticks = long.Parse(tokenExp);
            return ticks;
        }
		[HttpGet]
		[Route("checkLoginToken")]
		public async Task<IActionResult> CheckTokenIsValid(string token)
		{
			var tokenTicks = CheckLoginToken(token);
			var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

			var now = DateTime.Now.ToUniversalTime();
            var valid = tokenDate;
			if (valid >= now)
            {
				return Ok(true);
			}
            else
            {
                return Ok(false) ;
            }
		}
		[HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserInfoDTO userInfo)
        {
            try
            {

				var t = await this._loginService.Login(userInfo);
				return Ok(t);
                
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
		[HttpDelete]
		[Route("LogOut")]
		public async Task<IActionResult> LogOut(int userID)
		{
            try
            {
                this._connetionService.RemoveUserFromOnlineUsers(userID);
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
		}
	
		[HttpGet]
		[Route("GetConnectionPerUser")]
		public async Task<IActionResult> GetConnectionPerUser(int UserID)
		{
			try
			{
				var c = await this._connetionService.GetConnectionByUser(UserID);

				return new JsonResult(c);
			}
			catch
			{
				return BadRequest();
			}
		}

		
	}
}
