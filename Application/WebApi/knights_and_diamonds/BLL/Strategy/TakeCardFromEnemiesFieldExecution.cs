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

		public TakeCardFromEnemiesFieldExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(_context);
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

			var player = await this._unitOfWork.Player.GetPlayerWithFieldsAndHand(playerID);
			if(player == null)
			{
			
				throw new Exception("Player not found.");
			}

			var listOfPlayerFields = player.Fields.FindAll(f => f.CardOnField == null && f.FieldType=="MonsterField").ToList();
			if(listOfPlayerFields.Count < listOfFieldIDs.Count)
			{
				throw new Exception("Player don't have enough empty fields.");
			}

			foreach (var fieldID in listOfFieldIDs)
			{
				var cardField = await this._unitOfWork.CardField.GetCardField(fieldID);
				if (cardField == null)
				{
					throw new Exception("There is no field with this ID.");
				}
				if (cardField.CardOnField == null)
				{
					throw new Exception("There is no card on this field.");
				}
				if(cardField.FieldType != "MonsterField")
				{
					throw new Exception("You didn't select Monster card.");
				}

				var emptyField = player.Fields.Where(x => x.FieldType == "MonsterField" && x.CardOnField == null).FirstOrDefault();

				emptyField.CardPosition = cardField.CardPosition;
				emptyField.CardShowen = cardField.CardShowen;
				emptyField.CardOnField = cardField.CardOnField;

				cardField.CardPosition = true;
				cardField.CardShowen = true;
				cardField.CardOnField = null;

				this._unitOfWork.CardField.Update(emptyField);
				this._unitOfWork.CardField.Update(cardField);
			}
			await this._unitOfWork.Complete();
		}


	}
}
