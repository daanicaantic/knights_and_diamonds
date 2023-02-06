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
		public IRPSGamaeService _gameService { get; set; }
		public ConnectionsHub _connectedUsers { get; set; }

		public MyHub(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_userService = new UserService(this.context);
			_connetionService = new ConnectionService(this.context);
			_gameService = new RPSGameService(this.context);
			_connectedUsers = ConnectionsHub.GetInstance();
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

		public async Task GetOnlineUsers()
		{
			var onlineUsers = await this._connetionService.GetOnlineUsers();
			await Clients.All.SendAsync("GetUsersFromHub", onlineUsers);
		}

		public async Task GetPossibleLobbiesForUser(int userID)
		{
			var games = await this._gameService.LobbiesPerUser(userID);
			await Clients.All.SendAsync("GetUsersFromHub", games);
		}
	}
}
