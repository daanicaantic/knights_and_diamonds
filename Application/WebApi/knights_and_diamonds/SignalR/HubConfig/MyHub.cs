using BLL.Services;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;

using DAL.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static BLL.Services.Contracts.IGameService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SignalR.HubConfig
{
	public class MyHub : Hub
	{
		private readonly IConfiguration _config;

		private readonly KnightsAndDiamondsContext _context;
		public IUserService _userService { get; set; }
		public IPlayerService _playerService { get; set; }
		public IConnectionService _connectionService { get; set; }
		public IRPSGameService _rpsGameService { get; set; }
		public ILoginService _loginService { get; set; }
		public IGameService _gameService { get; set; }
		public ITurnService _turnService { get; set; }

		public ConnectionsHub _connectedUsers { get; set; }

		public MyHub(KnightsAndDiamondsContext context, IConfiguration config)
		{
			this._context = context;
			this._config = config;
			this._userService = new UserService(this._context);
			this._connectionService = new ConnectionService(this._context);
			this._rpsGameService = new RPSGameService(this._context);
			this._loginService = new LoginService(this._context, this._config);
			this._connectedUsers = ConnectionsHub.GetInstance();
			this._gameService = new GameService(this._context);
			this._playerService = new PlayerService(this._context);
			this._turnService = new TurnService(this._context);

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
		public void AddConnection(int userID)
		{
			this._connectionService.AddOnlineUser(userID, this.Context.ConnectionId);
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
			try
			{
                var onlineUsers = await this._connectionService.GetOnlineUsers();
                await Clients.All.SendAsync("GetUsersFromHub", onlineUsers);
            }
			catch(Exception ex)
			{
                Console.WriteLine("Error invoking GetOnlineUsers: {0}", ex.Message);
            }
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
			var lobbiesYouDelivered = this._rpsGameService.LobbiesPerUser(userID);
			var lobbiesYouCreated = this._rpsGameService.LobbiesYouCreated(userID);
			var connections = await this._connectionService.GetConnectionByUser(userID);
			foreach (var con in connections)
			{
				await Clients.Client(con).SendAsync("GetGamesRequests", lobbiesYouDelivered, lobbiesYouCreated);
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
		
		/*---------------------------K-N-I-G-H-T-S-A-N-D-D-I-A-M-O-N-D-S------------------------------*/

		public async Task GetHands(ConnectionsPerUser connections, List<MappedCard> playerHand)
		{
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetCardsInYourHand", playerHand);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetCardsInEnemiesHand", playerHand);
			}
		}
		public async Task GetGrave(ConnectionsPerUser connections,GraveToDisplay grave)
		{
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetGraveData", grave);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetGraveData", grave);
			}
		}
		public async Task GetTurnInfo(ConnectionsPerUser connections,TurnInfo turnInfo)
		{
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetTurnInfo", turnInfo);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetTurnInfo", turnInfo);
			}
		}
		public async Task GetDataAffterPlayCardWithEffect(ConnectionsPerUser connections, AffterPlaySpellTrapCardData data)
		{
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetAreaOfClicking", data);
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
		public async Task GetFieldsAbleToAttack(ConnectionsPerUser connections, List<int> fieldsAbleToAttack)
		{
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetFieldsAbleToAttack", fieldsAbleToAttack);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetFieldsAbleToAttack", fieldsAbleToAttack);
			}
		}
		public async Task GetStartingTurnInfo(int gameID,int playerID)
		{
			var game=await this._gameService.GetGameByID(gameID);
			if (game.PlayerOnTurn == playerID)
			{
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				var turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				await this.GetTurnInfo(connections, turnInfo);
			}
		}
		public async Task StartingDrawing(int gameID, int playerID)
		{
			try
			{
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				var player = await this._playerService.GetPlayer(playerID);
				if (player.GaemeStarted == false)
				{
					await this._playerService.SetGameStarted(player);
					await this._playerService.StartingDrawing(playerID);
				}

				var hand =await this._playerService.GetPlayersHand(playerID);
				await this.GetHands(connections, hand);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}


		public async Task DrawPhase(int gameID, int playerID)
		{
			try
			{
				var hand = await this._turnService.DrawPhase(gameID, playerID);
				var turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				await this.GetTurnInfo(connections, turnInfo);
				await this.GetHands(connections, hand);
				await this._turnService.ChangeToMainPhase(gameID);
				turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				await Task.Delay(1000);
				await this.GetTurnInfo(connections, turnInfo);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task BattlePhase(int gameID, int playerID)
		{
			try 
			{ 
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				var fieldsAbleToAttack=await this._turnService.BattlePhase(gameID, playerID);	
				var turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				await this.GetTurnInfo(connections, turnInfo);
				await this.GetFieldsAbleToAttack(connections, fieldsAbleToAttack);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task EndPhase(int gameID, int playerID,int enemiesID)
		{
			try
			{
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				var game = await this._turnService.EndPhase(gameID, playerID, enemiesID);
				game.PlayerOnTurn = enemiesID;
				var turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				await this.GetTurnInfo(connections, turnInfo);
				await this._turnService.NewTurn(game);
				turnInfo = await this._turnService.GetTurnInfo(gameID, playerID);
				await Task.Delay(1000);
				await this.GetTurnInfo(connections, turnInfo);
				await this.DrawPhase(gameID, enemiesID);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
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
		public async Task NormalSummon(int gameID, int playerID, int cardID, bool position)
		{
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
			var field=await this._gameService.NormalSummon(gameID,playerID,cardID,position);
			if (field == null)
			{
				throw new Exception("This player has no field");
			}
			await this.GetField(connections, field);
		}

		public async Task TributeSummon(List<int> fieldsIDs, int gameID, int playerID, int cardInDeckID, int numberOfStars, bool position)
		{
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
			var field = await this._gameService.TributeSummon(fieldsIDs,gameID,playerID, cardInDeckID,numberOfStars,position);
			var grave = await this._gameService.GetGamesGrave(gameID);
			if (field == null)
			{
				throw new Exception("This player has no field");
			}
			await this.GetField(connections, field);
			await this.GetGrave(connections, grave);
		}

		public async Task PlaySpellCard(int gameID, int playerID, int cardInDeckID, int cardEffectID)
		{
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
			var areaOfClicking = await this._gameService.PlaySpellCard(gameID, playerID, cardInDeckID, cardEffectID);

			var field = await this._gameService.GetPlayersField(playerID);
			if (field == null)
			{
				throw new Exception("This player has no field");
			}
			await this.GetField(connections, field);
			await this.GetDataAffterPlayCardWithEffect(connections,areaOfClicking);
		}
		public async Task ExecuteEffect(List<int> listOfCards, int cardFieldID, int playerID, int gameID)
		{
			try
			{
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				await this._gameService.ExecuteEffect(listOfCards, cardFieldID, playerID, gameID);
				var field = await this._gameService.GetPlayersField(playerID);
				var grave = await this._gameService.GetGamesGrave(gameID);
				if (field == null)
				{
					throw new Exception("This player has no field");
				}
				foreach (var item in field.CardFields)
				{
					Console.WriteLine(item.FieldIndex.ToString());
				}
				await Task.Delay(1000);
				await this.GetField(connections, field);
				await this.GetGrave(connections, grave);
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			
		}
		public async Task AttackEnemiesField(int gameID,int playerID,int fieldThatAtackID,int attackedFieldID) 
		{
			var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
			foreach (var con in connections.MyConnections)
			{
				await Clients.Client(con).SendAsync("GetFieldsIncludedInAttack",fieldThatAtackID, attackedFieldID);
			}
			foreach (var con in connections.EnemiesConnections)
			{
				await Clients.Client(con).SendAsync("GetFieldsIncludedInAttack", fieldThatAtackID, attackedFieldID);
			}
		}
		public async Task RemoveCardFromHandToGrave(int playerID,int cardID,int gameID)
		{
			try
			{
				var connections = await this._gameService.GameConnectionsPerPlayer(gameID, playerID);
				await this._gameService.RemoveCardFromHandToGrave(playerID, cardID, gameID);
				var hand = await this._playerService.GetPlayersHand(playerID);
				var grave = await this._gameService.GetGamesGrave(gameID);
				await this.GetHands(connections, hand);
				await this.GetGrave(connections, grave);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
	}
}
