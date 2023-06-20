using BLL.Factory;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DesignPatterns.Factory;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static BLL.Services.Contracts.IGameService;

namespace BLL.Services
{
	public class TurnService:ITurnService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IGameService _gameService { get; set; }
		public IPlayerService _playerService { get; set; }
		public ICardService _cardService { get;set; }
		public TurnService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_unitOfWork = new UnitOfWork(_context);
			this._gameService = new GameService(_context);
			this._cardService = new CardService(_context);
			this._playerService = new PlayerService(_context);
		}


		public async Task<Turn> NewTurn(Game game)
		{
			var turn = new Turn();
			game.Turns.Add(turn);
			game.TurnNumber = game.Turns.Count();
			this._unitOfWork.Game.Update(game);
			await this._unitOfWork.Complete();
			return turn;

		}
		public async Task<TurnInfo> GetTurnInfo(int gameID, int playerID)
		{
			var turnInfo = new TurnInfo();
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			turnInfo.PlayerOnTurn = game.PlayerOnTurn;
			turnInfo.TurnPhase = (int)this.GetTurnPhase(game);
			if (game.Turns?.LastOrDefault() == null)
			{
				turnInfo.IsMonsterSummoned = false;
			}
			else
			{
				turnInfo.IsMonsterSummoned = game.Turns.LastOrDefault().MonsterSummoned;
			}
			turnInfo.TurnNumber = game.TurnNumber;
			return turnInfo;
		}
		public async Task<List<MappedCard>> DrawPhase(int gameID,int playerID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			if (game.PlayerOnTurn != playerID)
			{
				throw new Exception("You are not on turn");
			}
			await this.NewTurn(game);
			await this._playerService.Draw(playerID);
			var hand = await this._playerService.GetPlayersHand(playerID);
			return hand;
		}
		public async Task ChangeToMainPhase(int gameID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			var turn = game.Turns?.LastOrDefault();
			if (turn == null)
			{
				throw new Exception("Error.There is no turn");
			}
			if (turn.DrawPhase == false)
			{
				throw new Exception("Error.You can only enter main phase if you are in draw phase");
			}
			turn.MainPhase = true;
			turn.DrawPhase = false;
			this._unitOfWork.Turn.Update(turn);
			await this._unitOfWork.Complete();
		}


		public async Task<List<int>> BattlePhase(int gameID,int playerID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			var currentTurn=game.Turns?.LastOrDefault();
			var playerFields = await this._unitOfWork.CardField.GetPlayerFields(playerID, "MonsterField");
			var listOfFieldsIDsReadyToAttack = new List<int>();
			if (game.PlayerOnTurn != playerID)
			{
				throw new Exception("You are not on turn");
			}
			if (currentTurn.MainPhase == false)
			{
				throw new Exception("You must be in main phase if you want to enter battle phase");
			}
			if (playerFields != null)
			{
				foreach (var field in playerFields)
				{
					if (field.CardOnField != null && field.CardPosition==true) {
						var fieldAbleToAttack = new AttackInTurn(true, currentTurn.ID,field.ID);
						await this._unitOfWork.AttackInTurn.Add(fieldAbleToAttack);
						listOfFieldsIDsReadyToAttack.Add(fieldAbleToAttack.CardFieldID);
					}
				}
			}
			currentTurn.MainPhase = false;
			currentTurn.BattlePhase = true;
			this._unitOfWork.Turn.Update(currentTurn);
			await this._unitOfWork.Complete();
			return listOfFieldsIDsReadyToAttack;
		}
		public async Task<Game> EndPhase(int gameID, int playerID,int enemiesID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			var enemiesPlayer = await this._unitOfWork.Player.GetOne(enemiesID);
			if (enemiesPlayer == null)
			{
				throw new Exception("There is no player with this id");
			}
			if (enemiesPlayer.GameID != gameID)
			{
				throw new Exception("There player is not in this game");
			}
			var currentTurn = game.Turns?.LastOrDefault();
			if (game.PlayerOnTurn != playerID)
			{
				throw new Exception("You are not on turn");
			}
			currentTurn.DrawPhase = false;
			currentTurn.MainPhase = false;
			currentTurn.BattlePhase = false;
			currentTurn.EndPhase = true;
			this._unitOfWork.Game.Update(game);
			await this._unitOfWork.Complete();
			return game;
		}
		public TurnPhase GetTurnPhase(Game game)
		{
			var turn = game.Turns?.LastOrDefault();
			if (turn == null)
			{
				return TurnPhase.NoPhase;
			}
			if (turn.DrawPhase)
			{
				return TurnPhase.DrawPhase;
			}
			else if (turn.MainPhase)
			{
				return TurnPhase.MainPhase;
			}
			else if (turn.BattlePhase)
			{
				return TurnPhase.BeatlePhase;
			}
			else if (turn.EndPhase)
			{
				return TurnPhase.EndPhase;
			}
			throw new Exception("There is some error");
		}

	}
}
