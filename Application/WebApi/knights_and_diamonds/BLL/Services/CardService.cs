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
using System.Linq.Expressions;
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
			await this.unitOfWork.Complete();
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

		public async Task RemoveCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Delete(card);
				await this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}
		public async Task UpdateCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Update(card);
				await this.unitOfWork.Complete();
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

        public async Task<List<MappedCard>> GetAllCards()
        {
			var cards = new List<MappedCard>();
			var spellTrapCards = await this.unitOfWork.Card.GetSpellTrapCards();
			var monsterCards = await this.unitOfWork.Card.GetMonsterCards();

			foreach (var card in spellTrapCards)
			{
				var c = new MappedCard(card.ID, card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID, card.ImgPath, card.Effect.Description);
				cards.Add(c);
			}
			foreach (var card in monsterCards)
			{
				var c = new MappedCard(card.ID,card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID,card.NumberOfStars,card.AttackPoints,card.DefencePoints, card.ImgPath, card.Effect.Description);
				cards.Add(c);
			}
			return cards;
		}

		public async Task<MappedCard> MapCard(CardInDeck cardInDeck)
		{
			var mappedCard = new MappedCard();
			if (cardInDeck.Card.Discriminator == "Card")
			{
				var card = await this.unitOfWork.Card.GetCard(cardInDeck.CardID);
				mappedCard = new MappedCard(card.ID, card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID, card.ImgPath, card.Effect.Description);
			}
			else
			{
				var card = await this.unitOfWork.Card.GetMonsterCard(cardInDeck.CardID);
				mappedCard = new MappedCard(card.ID, card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID, card.NumberOfStars, card.AttackPoints, card.DefencePoints, card.ImgPath, card.Effect.Description);
			}
			return mappedCard;
		}

        public async Task<List<MappedCard>> MapCards(List<CardInDeck> cardsInDeck)
        {
			var mappedCards = new List<MappedCard>();
			foreach (var cardInDeck in cardsInDeck)
			{
				mappedCards.Add(await this.MapCard(cardInDeck));
			}
			return mappedCards;
        }

/*		public async Task<CardDisplayDTO> MapMonsterCard(int cardID)
		{
            var mappedCard = new CardDisplayDTO();
            var card = await this.unitOfWork.Card.GetMonsterCard(cardID);
        }*/

/*		public async Task<List<CardDisplayDTO>> GetFilteredCards(int cardTypeID, string name)
		{
            *//*Expression<Func<T, bool>> predicate;*//*
            var cardType = await unitOfWork.Card.GetCardType(cardTypeID);

			
			//var cards = new List<CardDisplayDTO>();
			if(cardType == null && name == " ")
			{
				return await this.GetAllCards();
			}
			else if(cardType == null) 
			{
                Expression predicate = x => x.CardName == name;
				var cards = await unitOfWork.Card.Find(predicate);
			}
			
        }*/
    }
}
