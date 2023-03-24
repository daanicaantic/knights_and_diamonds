using DAL.DataContext;
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

		public CardRepository(KnightsAndDiamondsContext context) : base(context)
		{
			
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
			this.Context.Cards.Include(x=>x.Effect);
			this.Context.Cards.Add(card);
			return card;
		}

		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			this.Context.MonsterCards.Include(x=>x.Effect);
			this.Context.MonsterCards.Add(card);
			return card;
		}
		public async Task<Card> GetCardByName(string cardName)
		{
			var card=await this.Context.Cards.Where(x=>x.CardName == cardName).FirstOrDefaultAsync();
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
		public async Task<CardType> GetCardType(int cardTypeID)
		{
			var cardType = await this.Context.CardTypes.FindAsync(cardTypeID);
			return cardType;
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}
	}
}
