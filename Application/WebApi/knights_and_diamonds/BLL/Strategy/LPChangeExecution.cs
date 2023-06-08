using BLL.Services;
using BLL.Services.Contracts;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class LPChangeExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }

		public LPChangeExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			_playerService = new PlayerService(this._context);
			_gameService = new GameService(this._context);
			
		}

		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.NoChoose;
		}
		public async Task ExecuteEffect(List<int> listOfCards, Effect effect, int playerID, int gameID,int fieldID)
		{
			var player = await this._unitOfWork.Player.GetOne(playerID);
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			var game = await this._unitOfWork.Game.GetGameWithPlayers(player.GameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
			var enemiesPlayer = game?.Players?.Where(x => x.ID != playerID).FirstOrDefault();
			if (enemiesPlayer == null)
			{
				throw new Exception("There is no enemie in this game");
			}
			if (effect.EffectType.Type == "lpchangeAdd")
			{
				player.LifePoints = (int)(player.LifePoints + effect.PointsAddedLost);
				this._unitOfWork.Player.Update(player);
			}
			else if(effect.EffectType.Type == "lpchangeReduce")
			{
				enemiesPlayer.LifePoints= (int)(enemiesPlayer.LifePoints - effect.PointsAddedLost);
				this._unitOfWork.Player.Update(enemiesPlayer);
			}
			await this._gameService.RemoveCardFromFieldToGrave(fieldID, gameID, playerID);

		}
	}
}
