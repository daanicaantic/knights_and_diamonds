using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;

using DAL.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SignalR.HubConfig
{
	public class MyHub : Hub
	{
		private readonly IConfiguration _config;

		private readonly KnightsAndDiamondsContext context;
		public IUserService _userService { get; set; }
		public IPlayerService _playerService { get; set; }
		public IConnectionService _connectionService { get; set; }
		public IRPSGameService _rpsGameService { get; set; }
		public ILoginService _loginService { get; set; }
		public IGameService _gameService { get; set; }

		public ConnectionsHub _connectedUsers { get; set; }

		public MyHub(KnightsAndDiamondsContext context, IConfiguration config)
		{
			this.context = context;
			this._config = config;
			this._userService = new UserService(this.context);
			this._connectionService = new ConnectionService(this.context);
			this._rpsGameService = new RPSGameService(this.context);
			this._loginService = new LoginService(this.context, this._config);
			this._connectedUsers = ConnectionsHub.GetInstance();
			this._gameService = new GameService(this.context);
			this._playerService = new PlayerService(this.context);
		}
		public async Task askServer(string someTextForClient)
		{
			string tempstring;
			if (someTextForClient == "hey")
			{
				tempstring = "HI";
			}
			else
			{
				tempstring = "NEHI";
			}
			await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse", tempstring);
			Console.WriteLine(this.Context.ConnectionId);
		}
		public async Task GetConnection()
		{
			await Clients.Caller.SendAsync("GetConnectionID", this.Context.ConnectionId);
		}
		public async Task AddConnection(int userID)
		{
			this._connectionService.AddOnlineUser(userID, this.Context.ConnectionId);
			Console.WriteLine("Konaa" + this.context.ContextId);
		}
		public async Task LogIn(UserInfoDTO userInfo)
		{
			var t = await this._loginService.Login(userInfo);
			await Clients.Caller.SendAsync("GetUserToken", t);
		}
		public void Echo(string message)
		{
			Clients.All.SendAsync("Send", message);
		}

		public async Task GetOnlineUsers()
		{
			var onlineUsers = await this._connectionService.GetOnlineUsers();
			await Clients.All.SendAsync("GetUsersFromHub", onlineUsers);
		}

		public async Task CreateLobby(int player1ID, int player2ID)
		{
			var user1 = await this._userService.GetUserByID(player1ID);
			var user2 = await this._userService.GetUserByID(player2ID);

			var player1 = new OnlineUserDto(user1.ID, user1.Name, user1.SurName, user1.UserName);
			var player2 = new OnlineUserDto(user2.ID, user2.Name, user2.SurName, user2.UserName);

			await this._rpsGameService.NewLobby(player1, player2);

			await Clients.All.SendAsync("AddUsersToLobby");
		}

		public async Task GamesRequests(int userID)
		{
			var games = await this._rpsGameService.LobbiesPerUser(userID);
			var connections = await this._connectionService.GetConnectionByUser(userID);
			foreach (var con in connections)
			{
				await Clients.Client(con).SendAsync("GetGamesRequests", games);
			}
		}

		public async Task StartRPSGame(int rpsGameID)
		{
			var userIDs = await this._rpsGameService.RedirectToGame(rpsGameID);
			foreach (var userID in userIDs)
			{
				var connections = await this._connectionService.GetConnectionByUser(userID);
				foreach (var con in connections)
				{
					await Clients.Client(con).SendAsync("RPSGameStarted", rpsGameID);
				}
			}
		}

		public async Task CheckRPSWinner(int rpsGameID)
		{
			var RPSwinner = await this._rpsGameService.CheckRPSWinner(rpsGameID);
			var userIDs = await this._rpsGameService.RedirectToGame(rpsGameID);
			foreach (var userID in userIDs)
			{
				var connections = await this._connectionService.GetConnectionByUser(userID);
				foreach (var con in connections)
				{
					await Clients.Client(con).SendAsync("GetRPSWinner", RPSwinner);
				}
			}
		}
		/*---------------------------K-N-I-G-H-T-S-A-N-D-D-I-A-M-O-N-D-S----------------------------------------------*/

		public async Task GetHands(ConnectionsPerUser connections, List<MappedCard> playerHand)
		{
			var count = playerHand.Count;
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetFirstCards", playerHand);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetNumberOfCardsInHand", count);
			}
		}
		public async Task GetField(ConnectionsPerUser connections, FieldDTO field)
		{
			var enemiesField = this._gameService.GetEneiesField(field);
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetYourField", field);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetEnemiesField", enemiesField);
			}
		}

		public async Task StartingDrawing(int gameID, int playerID)
		{
			var player=await this._playerService.GetPlayer(playerID);
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);

			var countOfCards = 0;

			if (player.GaemeStarted == false)
			{
				await this._playerService.SetGameStarted(player);
				while (countOfCards < 5)
				{
					var card=await this._playerService.Draw(playerID);
					countOfCards = countOfCards + 1;
				}
			}
			var playerHand = await this._playerService.GetPlayersHand(playerID);
			await this.GetHands(connections, playerHand);
		}

		public async Task GetPlayersField(int gameID, int playerID)
		{
			
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
			var field = await this._gameService.GetPlayersField(playerID);
			if (field == null)
			{
				throw new Exception("This player has no field");
			}
			await this.GetField(connections,field);
		}


	}
}
