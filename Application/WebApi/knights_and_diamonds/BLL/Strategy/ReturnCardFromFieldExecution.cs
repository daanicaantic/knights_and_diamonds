using BLL.Services.Contracts;
using BLL.Services;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;
using DAL.Models;

namespace BLL.Strategy
{
	public class ReturnCardFromFieldExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public ReturnCardFromFieldExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(this._context);
		}
		public ChooseCardsFrom SelectCardsFrom()


		{
			return ChooseCardsFrom.Field;
		}

		public async Task ExecuteEffect(List<int> listOfFields, Effect effect, int playerID, int gameID,int fieldI)
		{
			if (effect.NumOfCardsAffected != listOfFields.Count)
			{
				throw new Exception("You didnt sellect all cards");
			}
			foreach (var fieldID in listOfFields)
			{
				var cardField = await this._unitOfWork.CardField.GetCardField(fieldID,playerID);
				if (cardField == null)
				{
					throw new Exception("There is no field with this ID");
				}
				if(cardField.CardOnField == null)
				{
					throw new Exception("There is no card on this field");
				}
				var playerField = await this._unitOfWork.Player.GetPlayersField(playerID);
				if (playerField == null)
				{
					throw new Exception("There is no player with this ID");
				}
				playerField.Hand.CardsInHand.Add(cardField.CardOnField);
				cardField.CardOnField = null;
				this._unitOfWork.Player.Update(playerField);
				await this._unitOfWork.Complete();
			}	
		}

		public string WhenCanYouActivateTrapCard()
		{
			throw new NotImplementedException();
		}
	}
}
