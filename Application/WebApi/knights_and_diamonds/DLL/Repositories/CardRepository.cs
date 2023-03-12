using DAL.DataContext;
using DAL.DesignPatterns.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class CardRepository : Repository<Card>, ICardRepository
	{
		public IDescriptionFactory _descriptionFactory { get; set; }
		public IFactory _factory { get; set; }

		public CardRepository(KnightsAndDiamondsContext context) : base(context)
		{
			_descriptionFactory = new ConcreteDescriptionFactory();
		}
		public IQueryable<Card> GetCardsPerPage(int pageIndex, int pageSize)
		{
			return (IQueryable<Card>)Context.Cards
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToList();
		}
		public async Task<Card> AddCard(Card card)
		{
			var ct = await this.Context.CardTypes.FindAsync(card.CardTypeID);
			if (ct == null) {
				throw new Exception("There is not cardType with this ID");
			}
			this.Context.Cards.Include(x => x.CardType).Include(x=>x.Effect);
			card.CardType = ct;
			this.Context.Cards.Add(card);
			return card;
		}

		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			var ct = await this.Context.CardTypes.FindAsync(card.CardTypeID);
			if (ct == null)
			{
				throw new Exception("There is not cardType with this ID");
			}
			var mt = await this.Context.MonsterTypes.FindAsync(card.MonsterTypeID);
			if (mt == null)
			{
				throw new Exception("There is not monsterType with this ID");
			}
			var et = await this.Context.ElementTypes.FindAsync(card.ElementTypeID);
			if (et == null)
			{
				throw new Exception("There is not elementType with this ID");
			}
			this.Context.MonsterCards.Include(x => x.CardType).Include(x => x.MonsterType).Include(x => x.ElementType);
			card.CardType = ct;
			card.MonsterType = mt;
			card.ElementType = et;
			this.Context.MonsterCards.Add(card);
			return card;
		}
		public async Task<Effect> AddEffect(Effect effect) 
		{
			this.Context.Effects.Include(x => x.EffectType).Include(x => x.Description);

			/*this._factory = this._descriptionFactory.FactoryMethod(effect.EffectType.Type);
			var description = await this._factory.GetDescription();*/

			var effectType = await this.Context.EffectTypes.FindAsync(effect.EffectTypeID);
			if (effectType == null)
			{
				throw new Exception("There is not effectType with this ID");
			}
			effect.EffectType = effectType;
			return effect;
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}
	}
}
