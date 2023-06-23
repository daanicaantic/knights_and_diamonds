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
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services
{
	public class RPSGameService : IRPSGameService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		private InGameUsers _usersInGame { get; set; }
		public RPSGameService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._usersInGame = InGameUsers.GetInstance();
		}

		public int NewLobby(OnlineUserDto user, OnlineUserDto challengedUser)
		{
			if (!this._usersInGame.UsersInGame.Contains(user.ID) && !this._usersInGame.UsersInGame.Contains(challengedUser.ID))
			{
				Lobby lobby;
				var lobbies = this._usersInGame.Lobbies;

				if (lobbies.Any(x => x.User1.ID == user.ID && x.User2.ID == challengedUser.ID))
				{
					throw new Exception("You have allready challenged this player");
				}

                if (lobbies.Any(x => x.User1.ID == challengedUser.ID && x.User2.ID == user.ID))
                {
                    throw new Exception("This user allready challenged you");
                }

                int lobbyID = this._usersInGame.lobbyID++;

                if (!lobbies.Any(x => x.ID == lobbyID))
				{
					lobby = new Lobby(lobbyID, user, challengedUser);
					lobbies.Add (lobby);
				}

				else
				{
					throw new Exception("There is already lobby with this ID");
				}

				return lobby.ID;
			}
			else 
			{ 
				throw new Exception("One or both users are already in game");
			}
		}

		public async Task<int> StartGame(int lobbyID)
		{
			var lobbies = this._usersInGame.Lobbies;
			if (lobbies == null)
			{
				throw new Exception("There are no any loby");
			}

			if (!lobbies.Any(x => x.ID == lobbyID))
			{
				throw new Exception("There is no lobby with this ID");
			}

			var lobby = lobbies.Find(x => x.ID == lobbyID);

			if (lobby.User1 == null && lobby.User2 == null)
			{
				throw new Exception("There is no users in this lobby");
			}

            var user1 = await this._unitOfWork.User.GetOne(lobby.User1.ID);
			if (this._usersInGame.UsersInGame.Contains(user1.ID))
			{
				throw new Exception(user1.UserName + "is already in game.");
			}

			var deck1 = await this._unitOfWork.Deck.GetCardsFromDeck(user1.MainDeckID, user1.ID);
			if (deck1 == null) 
			{
				throw new Exception(user1.UserName + " didn't have deck to play with");
			}
            
            var user2 = await this._unitOfWork.User.GetOne(lobby.User2.ID);
			if (this._usersInGame.UsersInGame.Contains(user2.ID))
			{
				throw new Exception(user2.UserName + "is already in game.");
			}

			var deck2 = await this._unitOfWork.Deck.GetCardsFromDeck(user2.MainDeckID, user2.ID);
			if (deck2 == null)
			{
				throw new Exception(user2.UserName + " didn't have deck to play with");
			}

			RockPaperScissorsGame rpsGame = new RockPaperScissorsGame();
			Game cardGame = new Game();
			
			await this._unitOfWork.RPSGame.Add(rpsGame);

			Player player1 = new Player(rpsGame, cardGame, user1, deck1);
			player1.CreateFields();
			Player player2 = new Player(rpsGame, cardGame, user2, deck2);
			player2.CreateFields();

			this._usersInGame.UsersInGame.Add(user1.ID);
			this._usersInGame.UsersInGame.Add(user2.ID);

			await this._unitOfWork.Player.Add(player1);
			await this._unitOfWork.Player.Add(player2);
			this._usersInGame.Lobbies.Remove(lobby);

			await this._unitOfWork.Complete();

			return rpsGame.ID;
		}
        public int DenyGame(int lobbyID)
        {
            var lobbies = this._usersInGame.Lobbies;

            var lobby = lobbies.Find(x => x.ID == lobbyID);
			if (lobby == null)
			{
				throw new Exception("There is no lobby with this ID");

			}

			lobbies.Remove(lobby);

            return lobby.ID;
        }
        public Dictionary<int,List<int>> GetGames()
		{
			try
			{
				return this._usersInGame.GamesIDs;
			}
			catch
			{
				throw;
			}
		}
		public async Task<List<int>> RedirectToGame(int gameID) 
		{
			List<int> usersIDs=new List<int>(); 
			var game = await this._unitOfWork.RPSGame.GetGameWithPlayers(gameID);

			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
		
			foreach (var player in game.Players)
			{
				usersIDs.Add(player.UserID);
			}
			return usersIDs;
		}

		public List<Lobby> LobbiesPerUser(int userID)
		{
			var lobbies = this._usersInGame.Lobbies.Where(x => x.User2.ID == userID).ToList();
			return (lobbies);
		}

        public List<Lobby> LobbiesYouCreated(int userID)
        {
            var lobbies = this._usersInGame.Lobbies.Where(x => x.User1.ID == userID).ToList();
            return (lobbies);
        }

        public async Task<GameDTO> GetGame(int gameID,int userID)
        {
			GameDTO game = new GameDTO();
			var rpsGame = await this._unitOfWork.RPSGame.GetGameWithPlayers(gameID);
			if (rpsGame == null)
			{
				throw new Exception("Wrong gameID");
			}
			foreach (var player in rpsGame.Players)
			{
				if (player.UserID == userID)
				{
					game.PlayerID = player.ID;
					game.GameID = player.GameID;
				}
				else
				{
					game.EnemiePlayerID = player.ID;
				}
			}
			return game;
		}
		public async Task PlayMove(int playerID, string moveName)
		{
			var player = await this._unitOfWork.Player.GetOne(playerID);

			if (player == null)
			{
				throw new Exception("Player is undefined.");
			}

			if (moveName == "Rock") 
			{
				player.Play = Play.Rock;
			}
			else if (moveName == "Paper")
			{
				player.Play = Play.Paper;
            }
			else if (moveName == "Scissors")
			{
				player.Play = Play.Scissors;
			}
			else 
			{
				throw new Exception("Move is undefined.");
			}

			this._unitOfWork.Player.Update(player);
			await this._unitOfWork.Complete();
        }
		public async Task<int> CheckRPSWinner(int RPSgameID)
		{
            var game = await this._unitOfWork.RPSGame.GetGameWithPlayers(RPSgameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
			var players = game.Players;
			var cardGame = await this._unitOfWork.Game.GetOne(players[0].GameID);
			if (cardGame == null)
			{
				throw new Exception("There is no game with this ID");
			}

			int winner=new();


			if (game == null)
			{
				throw new Exception("RPS game not found.");
			}

			if (players[0].Play == players[1].Play)
			{	
				return 0;
				//nereseno
			}
			else if (players[0].Play == null)
			{
				winner = players[1].ID;
				//izazvani je pobednik (player2)
			}
			else if (players[1].Play == null)
			{
				winner = players[0].ID;
				//izazivac je pobednik (player1)
            }
            else if (players[0].Play == Play.Rock)
			{
				if (players[1].Play == Play.Paper)
				{
                    winner= players[1].ID;
                    //izazvani je pobednik (player2)
                }
                else if (players[1].Play == Play.Scissors)
				{
                    winner= players[0].ID;
                    //izazivac je pobednik (player1)
                }
                else
				{
					throw new Exception("Player move is undefined");
				}
			}
			else if (players[0].Play == Play.Scissors)
			{
				if (players[1].Play == Play.Rock)
				{
                    winner= players[1].ID;
                    //izazvani je pobednik (player2)
                }
                else if (players[1].Play == Play.Paper)
				{
                    winner= players[0].ID;
                    //izazivac je pobednik (player1)
                }
                else
				{
					throw new Exception("Player move is undefined");
				}
			}
			else if (players[0].Play == Play.Paper)
			{
				if (players[1].Play == Play.Scissors)
				{
                    winner= players[1].ID;
                    //izazvani je pobednik (player2)
                }
                else if (players[1].Play == Play.Rock)
				{
                    winner= players[0].ID;
                    //izazivac je pobednik (player1)
                }
                else
				{
					throw new Exception("Player move is undefined");
				}
			}

			game.Winner = winner;
			cardGame.PlayerOnTurn = winner;
			this._unitOfWork.Game.Update(cardGame);
			this._unitOfWork.RPSGame.Update(game);
			await this._unitOfWork.Complete();
			return winner;
        }


        public async Task<Player> GetPlayer(int gameID, int userID)
        {
			var player = await this._unitOfWork.Player.GetPlayer(gameID, userID);
			if (player == null)
			{
				throw new Exception("Player is undefined.");
			}
			return player;
        }
		public async Task<string> GetPlayersMove(int playerID)
		{
			var player = await this._unitOfWork.Player.GetPlayerByID(playerID);
			if (player == null)
			{
				throw new Exception("Player is undefined.");
			}
			switch (player.Play)
			{
				case Play.Rock:return "Rock";
				case Play.Scissors: return "Scissors";
				case Play.Paper: return "Paper";
				default:return "Undefined";
			}
		}
		public void RemoveUserFromUsersInGame(int userID)
        {
			if(this._usersInGame.UsersInGame.Contains(userID))
			{
                this._usersInGame.UsersInGame.Remove(userID);
            }
        }
    }
}
