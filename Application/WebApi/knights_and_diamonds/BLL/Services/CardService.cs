using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class CardService : ICardService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		public IEffectFactory _effectFactory { get; set; }
		public IFactory _factory { get; set; }
		public IEffectService _effectService { get; set; }


		public CardService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this.unitOfWork = new UnitOfWork(_context);
			this._effectService = new EffectService(_context);
			this._effectFactory = new ConcreteEffectFactory();
			
		}

		public async Task AddCard(CardDTO card)
		{
		/*	var doesCardExists = await this.unitOfWork.Card.GetCardByName(card.CardName);
			if (doesCardExists != null)
			{
				throw new Exception("Card with this name already exists");
			}*/
			var effectType = await this.unitOfWork.Effect.GetEffectType(card.EffectTypeID);
			if (effectType == null)
			{
				throw new Exception("There is no EffectType with this ID");
			}
			var type = this.SplitType(effectType.Type);
			var effect = this.GenerateEffect(type, effectType.Type, card.NumOfCardsAffected, card.PointsAddedLost);
			effect.EffectTypeID = card.EffectTypeID;
			var checkIfEffectExist = await this.unitOfWork.Effect.GetEffectByDescription(effect.Description);
			if (checkIfEffectExist != null)
			{
				effect = checkIfEffectExist;
			}
			var cardType = await this.unitOfWork.Card.GetCardType(card.CardTypeID);
			if (cardType == null)
			{
				throw new Exception("There is no CardType with this ID");
			}
			if (cardType.Type == "MonsterCard")
			{
				MonsterCard monsterCard = new MonsterCard(card.CardName, card.ImgPath, card.CardLevel, card.AttackPoints, card.DefencePoints, effect, card.CardTypeID);
				await this.unitOfWork.Card.AddMonsterCard(monsterCard);
			}
			else
			{
				Card c = new Card(card.CardName, card.ImgPath, card.CardTypeID, effect);
				await this.unitOfWork.Card.AddCard(c);
			}
			this.unitOfWork.Complete();
		}
		public async Task<Card> GetCard(int id)
		{
			try
			{
				return await this.unitOfWork.Card.GetOne(id);
			}
			catch
			{
				throw;
			}
		}

		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			try
			{
				var c = await this.unitOfWork.Card.AddMonsterCard(card);
				this.unitOfWork.Complete();
				return c;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public void RemoveCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Delete(card);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}
		public void UpdateCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Update(card);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}

		public IQueryable<Card> FindCardByName(string name)
		{
			try
			{
				return this.unitOfWork.Card.Find(x => x.CardName == name);
			}
			catch
			{
				throw;
			}
		}
		public string SplitType(string effectType)
		{
			string newstring = "";
			int i = 0;
			while (!effectType.Length.Equals(i) && !char.IsUpper(effectType[i]))
			{
				newstring += effectType[i].ToString();
				i++;
			}

			return newstring;
		}
		
		public Effect GenerateEffect(string type,string concreteType,int numOfCardAffected,int pointsAddedLost)
		{
			this._factory = this._effectFactory.FactoryMethod(type, concreteType, numOfCardAffected, pointsAddedLost);
			var effect = this._factory.GetEffect();
			return effect;
		}

        public async Task<List<Card>> GetAllCards()
        {
           return await this.unitOfWork.Card.GetAllCards();
        }
    }
}
