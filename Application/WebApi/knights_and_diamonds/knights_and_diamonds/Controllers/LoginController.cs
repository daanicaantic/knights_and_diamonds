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

namespace knights_and_diamonds.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly KnightsAndDiamondsContext context;
        public IUserService _userService { get; set; }
        public LoginController(KnightsAndDiamondsContext context, IConfiguration config)
        {
            this.context = context;
            _userService = new UserService(this.context);
            _config = config;
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

                return Ok(t);
            }

            return NotFound("This user does not exists!");
        }
    }
}
