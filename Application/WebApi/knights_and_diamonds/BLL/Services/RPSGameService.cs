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
using DAL.DTOs;

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


		public void NewLobby(OnlineUserDto user, OnlineUserDto challengedUser)
		{
			if (!this._usersingame.UsersInGame.Contains(user.ID) && !this._usersingame.UsersInGame.Contains(challengedUser.ID))
			{
				List<OnlineUserDto> users;
				int lobbyID =this._usersingame.lobbyID++;

				if (!this._usersingame.LobbyIDs.ContainsKey(lobbyID))
				{
					users = new List<OnlineUserDto>();
					users.Add(user);
					users.Add(challengedUser);
					this._usersingame.LobbyIDs.Add(lobbyID, users);
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
				List<OnlineUserDto> users;
				List<int> usersIDs=new List<int>();

				var game = new RockPaperScissorsGame();
				this.unitOfWork.RPSGame.Add(game);


				users = this._usersingame.LobbyIDs[lobbyID].ToList();
				Console.WriteLine(Serialize(users));
				
				foreach (var u in users)
				{
					var user=await this.unitOfWork.User.GetOne(u.ID);
					if(user != null) 
					{ 
						this._usersingame.UsersInGame.Add(user.ID);
						usersIDs.Add(user.ID);
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
		public async Task<Dictionary<int, List<OnlineUserDto>>> LobbiesPerUser(OnlineUserDto user)
		{
			try
			{
				Dictionary<int,List<OnlineUserDto>> pom;
				List<OnlineUserDto> onlineUsers;
				pom = new Dictionary<int, List<OnlineUserDto>>();
				foreach (var item in this._usersingame.LobbyIDs)
				{
					if(item.Value.Any(x=>x.ID == user.ID)) 
					{ 
						pom.Add(item.Key, item.Value);
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
