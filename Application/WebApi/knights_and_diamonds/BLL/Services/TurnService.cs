using BLL.Factory;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DesignPatterns.Factory;
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
		public TurnService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_unitOfWork = new UnitOfWork(_context);
		}


		public async Task<TurnInfo> NewTurn(int gameID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);


			/*  if (game.TurnNumber != 0)
			  {
				  var lastturn = game.Turns.Last();
				  if (lastturn.EndPhase == false)
				  {
					  throw new Exception("Last turn is not finnished yet");
				  }
			  }*/
			var turn = new Turn();
			game.Turns.Add(turn);
			game.TurnNumber = game.Turns.Count();
			this._unitOfWork.Game.Update(game);
			await this._unitOfWork.Complete();

			var turnInfo = new TurnInfo();
			turnInfo.PlayerOnTurn = game.PlayerOnTurn;
			turnInfo.TurnPhase = (int)await this.GetTurnPhase(game);
			return turnInfo;

		}
		public async Task<TurnInfo> GetTurnInfo(int gameID, int playerID)
		{
			var turnInfo = new TurnInfo();
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			if (game.Turns.Count == 0)
			{
				turnInfo=await this.NewTurn(gameID);
				return turnInfo;
			}
			turnInfo.PlayerOnTurn = game.PlayerOnTurn;
			turnInfo.TurnPhase = (int)await this.GetTurnPhase(game);
			turnInfo.IsMonsterSummoned = game.Turns.LastOrDefault().MonsterSummoned;
			turnInfo.TurnNumber = game.TurnNumber;
			return turnInfo;
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

		public async Task<TurnPhase> GetTurnPhase(Game game)
		{
			var turn = game.Turns?.LastOrDefault();
			if (turn == null)
			{
				throw new Exception("There is no turns in this game");
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
