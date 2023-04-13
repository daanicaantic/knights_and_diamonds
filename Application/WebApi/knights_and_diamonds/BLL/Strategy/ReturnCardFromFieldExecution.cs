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
			_playerService = new PlayerService(this._context);
		}
		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.Field;
		}

		public async Task ExecuteEffect(List<int> listOfCards, string description, int playerID)
		{
			var effect = await this.GetEffect(description);
			if (effect.NumOfCardsAffected != listOfCards.Count)
			{
				throw new Exception("You didnt sellect all cards");
			}
			foreach (var fieldID in listOfCards)
			{
				var cardField = await this._unitOfWork.CardField.GetCardField(fieldID);
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
		public async Task<Effect> GetEffect(string description)
		{
			var effect = await this._unitOfWork.Effect.GetEffectByDescription(description);
			if (effect == null)
			{
				throw new Exception("There is no effect with this description");
			}
			if (effect.EffectType.Type != "returnCardFromFieldToHand")
			{
				throw new Exception("There is some Error");
			}
			return effect;
		}
	}
}
