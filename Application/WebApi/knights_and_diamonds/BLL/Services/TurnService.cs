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
			return turnInfo;
		}

		public async Task<TurnPhase> GetTurnPhase(Game game)
		{
			var turn = game.Turns.LastOrDefault();
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
