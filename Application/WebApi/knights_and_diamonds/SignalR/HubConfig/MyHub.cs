using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubConfig
{
	public class MyHub:Hub
	{
		private readonly KnightsAndDiamondsContext context;
		public IUserService _userService { get; set; }
		public IConnectionService _connetionService { get; set; }

		public MyHub(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_userService = new UserService(this.context);
			_connetionService = new ConnectionService(this.context);
		}
		public async Task askServer(string someTextForClient) 
		{
			string tempstring;
			if(someTextForClient == "hey") 
			{
				tempstring = "HI";
			}
			else 
			{
				tempstring = "NEHI";
			}
			await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse",tempstring);
		}
		public void Echo(string message)
		{
			Clients.All.SendAsync("Send", message);
		}
		public async Task getOnlineUsers()
		{
			var c = await this._connetionService.GetAllConnections();
			var users = new List<OnlineUserDto>();
			foreach (var item in c)
			{
				var time = item.isStillLogeniIn.AddHours(1);
				if (DateTime.UtcNow>time) 
				{
					Console.WriteLine(item.isStillLogeniIn.AddHours(1));
					Console.WriteLine(DateTime.UtcNow);
					c = c.Where(p => p != item);
					this._connetionService.RemoveConnection(item);
				}
				else
				{
					var user = await this._userService.GetUserByID(item.UserID);
					var onlineuserDTO = new OnlineUserDto(user.ID, user.Name, user.SurName, user.UserName);
					users.Add(onlineuserDTO);
				}
			}
			await Clients.All.SendAsync("GetUsersFromHub", users);
		}
	}
}
