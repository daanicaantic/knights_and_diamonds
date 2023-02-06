using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DesignPatterns;
using System.Numerics;
using System.Collections;
using static System.Text.Json.JsonSerializer;



namespace BLL.Services
{
	public class RPSGameService : IRPSGamaeService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		private InGameUsers _usersingame { get; set; }
		public RPSGameService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			_usersingame = InGameUsers.GetInstance();
		}


		public void NewLobby(User user, User challengedUser)
		{
			if (!this._usersingame.UsersInGame.Contains(user.ID) && !this._usersingame.UsersInGame.Contains(challengedUser.ID))
			{
				List<int> usersIDs;
				int lobbyID =this._usersingame.lobbyID++;

				if (!this._usersingame.LobbyIDs.ContainsKey(lobbyID))
				{
					usersIDs = new List<int>();
					usersIDs.Add(user.ID);
					usersIDs.Add(challengedUser.ID);
					this._usersingame.LobbyIDs.Add(lobbyID, usersIDs);
					Console.WriteLine(Serialize(this._usersingame.LobbyIDs.ToList()));
				}
				else
				{
					throw new Exception("There is not game with this ID");
				}

			}
			else 
			{ 
				throw new Exception("One or both users are already in game");
			}

		}

		public async Task StartGame(int lobbyID)
		{
			if (this._usersingame.LobbyIDs.ContainsKey(lobbyID)) 
			{
				PreGameSession player;
				List<int> usersIDs;

				var game = new RockPaperScissorsGame();
				this.unitOfWork.RPSGame.Add(game);


				usersIDs = this._usersingame.LobbyIDs[lobbyID].ToList();
				Console.WriteLine(Serialize(usersIDs));
				
				foreach (var userID in usersIDs)
				{
					var user=await this.unitOfWork.User.GetOne(userID);
					if(user != null) 
					{ 
						this._usersingame.UsersInGame.Add(user.ID);
					}
					else
					{
						throw new Exception("There is no user with this ID");
					}
					player = new PreGameSession(game, user);
					this.unitOfWork.PreGame.Add(player);
				}
				this.unitOfWork.Complete();
				this._usersingame.GamesIDs.Add(game.ID, usersIDs);
				this._usersingame.LobbyIDs.Remove(lobbyID);

				Console.WriteLine(Serialize(this._usersingame.GamesIDs.ToList()));
				Console.WriteLine(Serialize(this._usersingame.UsersInGame));
			}
			else 
			{
				throw new Exception("There is no lobby with this lobbyID");
			}
			
		}
		public async Task<Dictionary<int,List<int>>> GetGames()
		{
			try
			{
				return this._usersingame.GamesIDs;
			}
			catch
			{
				throw;
			}
		}
		public async Task<Dictionary<int, List<int>>> LobbiesPerUser(int userID)
		{
			try
			{
				Dictionary<int,List<int>> pom;
				pom = new Dictionary<int, List<int>>();
				foreach (var item in this._usersingame.LobbyIDs)
				{
					if (item.Value.Contains(userID)) 
					{
						pom.Add(item.Key,item.Value);
					}
				}
				return pom;
			}
			catch
			{
				throw;
			}
		}
	}
}
