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

namespace knights_and_diamonds.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly KnightsAndDiamondsContext context;
        public IUserService _userService { get; set; }
		public IConnectionService _connetionService { get; set; }

		public LoginController(KnightsAndDiamondsContext context, IConfiguration config)
        {
            this.context = context;
            _userService = new UserService(this.context);
			_connetionService=new ConnectionService(this.context);
			_config = config;
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
        [HttpGet]
        [Route("getConnection")]
        public async Task<IActionResult> getConnection(int id)
        {
            var c = await this._connetionService.GetConnection(id);
            return new JsonResult(c);

        }

		[HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] UserInfoDTO userInfo)
        {
            TokenDTO t = new TokenDTO();
            if (userInfo == null || string.IsNullOrEmpty(userInfo.Email)
                || string.IsNullOrEmpty(userInfo.Password))
                return BadRequest("Data not valid!");

            var user =  this._userService.GetUser(userInfo.Email, userInfo.Password).FirstOrDefault();

            var claims = new List<Claim>();

            if (user != null)
            {
                claims.Add(new Claim("ID", user.ID.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.Name));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: signIn);
                t.Token = new JwtSecurityTokenHandler().WriteToken(token);
                t.Role = user.Role;
                t.ID = user.ID;

                Connection c = new Connection();
                c.UserID = user.ID;
                c.isStillLogeniIn = DateTime.UtcNow;

                Console.WriteLine(c.isStillLogeniIn);

				this._connetionService.AddConnection(c);
                return Ok(t);
            }

            return NotFound("This user does not exists!");
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
		[Route("GetAllConection")]
		public async Task<IActionResult> GetConnection()
		{
			try
			{
				var c = await this._connetionService.GetAllConnections();

                return new JsonResult(c);
			}
			catch
			{
				return BadRequest();
			}
		}
		[HttpGet]
		[Route("GetOnlineUsers")]
		public async Task<IActionResult> GetOnlineUsers()
		{
			try
			{
				var c = await this._connetionService.GetOnlineUsers();

				return new JsonResult(c);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
