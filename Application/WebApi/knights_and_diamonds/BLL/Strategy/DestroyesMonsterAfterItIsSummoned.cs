using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;
using BLL.Strategy.Context;

namespace BLL.Strategy
{
	public class DestroyesMonsterAfterItIsSummoned:IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }

		public DestroyesMonsterAfterItIsSummoned(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(this._context);
			this._gameService = new GameService(this._context);

		}

		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.NoChoose;
		}
		public async Task ExecuteEffect(List<int> listOfFields, Effect effect, int playerID, int gameID, int fieldID)
		{
			var enemiesID = await this._unitOfWork.Game.GetEnemiesPlayerID(gameID, playerID);
			await this._gameService.RemoveCardFromFieldToGrave(listOfFields[0], gameID, enemiesID);
			await this._gameService.RemoveCardFromFieldToGrave(fieldID, gameID, playerID);
		}

		public string WhenCanYouActivateTrapCard()
		{
			return "AfterMonsterIsSummoned";
		}
	}
}
