using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;

namespace SignalR.GameHub
{
	public class GameHub:Hub
	{
		private readonly KnightsAndDiamondsContext context;
		public IRPSGamaeService _gameService { get; set; }

		public GameHub(KnightsAndDiamondsContext context)
		{
			this.context = context;
			_gameService = new RPSGameService(this.context);
		}
		public async Task GetPossibleLobbiesForUser(int userID)
		{
			var games =await this._gameService.LobbiesPerUser(userID);
			await Clients.All.SendAsync("GetUsersFromHub", games);
		}
	}
}
