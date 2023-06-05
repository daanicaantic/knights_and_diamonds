using BLL.Services;
using BLL.Services.Contracts;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class DrawExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }
		public DrawExecution(KnightsAndDiamondsContext context)
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
		public async Task ExecuteEffect(List<int> listOfCards,Effect effect,int playerID,int gameID,int fieldID)
		{
			int cardsToDraw = 0;
			while (cardsToDraw!=effect.PointsAddedLost)
			{
				cardsToDraw++;
				await this._playerService.Draw(playerID);
			}

			await this._gameService.RemoveCardFromFieldToGrave(fieldID, gameID,playerID);
		}
	}
}
