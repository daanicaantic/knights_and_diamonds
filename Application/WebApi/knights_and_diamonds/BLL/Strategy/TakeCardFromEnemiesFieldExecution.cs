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
	public class TakeCardFromEnemiesFieldExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }

		public TakeCardFromEnemiesFieldExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(_context);
			_gameService = new GameService(this._context);

		}
		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.EnemiesField;
		}

		public async Task ExecuteEffect(List<int> listOfFieldIDs, Effect effect, int playerID,int gameID, int fieldI)
		{
			if(effect.NumOfCardsAffected != listOfFieldIDs.Count)
			{
				throw new Exception("You didn't select enough cards.");
			}
			var listOfPlayerFields=await this._unitOfWork.CardField.GetEmptyPlayerFields(playerID, "MonsterField");
			if(listOfPlayerFields.Count < listOfFieldIDs.Count)
			{
				throw new Exception("Player don't have enough empty fields.");
			}
			var enemiesID = await this._unitOfWork.Game.GetEnemiesPlayerID(gameID, playerID);
			foreach (var fieldID in listOfFieldIDs)
			{
				var cardField = await this._unitOfWork.CardField.GetCardField(fieldID,enemiesID);
				if (cardField.FieldType != "MonsterField")
				{
					throw new Exception("You didn't select Monster card.");
				}
				if (cardField.CardOnField == null)
				{
					throw new Exception("There is no card on this field.");
				}
				var emptyField = listOfPlayerFields.FirstOrDefault();

				emptyField.CardPosition = cardField.CardPosition;
				emptyField.CardShowen = cardField.CardShowen;
				emptyField.CardOnField = cardField.CardOnField;

				cardField.CardPosition = true;
				cardField.CardShowen = true;
				cardField.CardOnField = null;

				this._unitOfWork.CardField.Update(emptyField);
				this._unitOfWork.CardField.Update(cardField);
			}
			await this._gameService.RemoveCardFromFieldToGrave(fieldI, gameID, playerID);
			await this._unitOfWork.Complete();
		}


	}
}
