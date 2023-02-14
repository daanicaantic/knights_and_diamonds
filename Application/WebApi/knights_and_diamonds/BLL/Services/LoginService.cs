using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using System.Threading.Tasks;

namespace BLL.Services
{
	public class LoginService : ILoginService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		public OnlineUsers _onlineUsers { get; set; }
		private readonly IConfiguration _config;


		public LoginService(KnightsAndDiamondsContext context, IConfiguration config)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			_onlineUsers = OnlineUsers.GetInstance();
			_config = config;
		}

		public async Task<TokenDTO> Login(UserInfoDTO userInfo)
		{
			TokenDTO t = new TokenDTO();

			var user = this.unitOfWork.User.Find(x => x.Email == userInfo.Email && x.Password == userInfo.Password).FirstOrDefault();

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

				return t;
			}
			else
			{
				throw new Exception("Error");

			}

		}
	} 
} 