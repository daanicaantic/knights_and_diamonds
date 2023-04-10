using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
//#nullable disable

namespace DAL.Repositories
{
	public class CardRepository : Repository<Card>, ICardRepository
	{

		public CardRepository(KnightsAndDiamondsContext context) : base(context)
		{
			
		}

        public KnightsAndDiamondsContext Context
        {
            get { return _context as KnightsAndDiamondsContext; }
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
			this.Context.Cards.Include(x => x.Effect);
			await this.Context.Cards.AddAsync(card);
			return card;
		}

		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			this.Context.MonsterCards.Include(x => x.Effect);
			await this.Context.MonsterCards.AddAsync(card);
			return card;
		}

		public async Task<Card> GetCardByName(string cardName)
		{
			var card = await this.Context.Cards.Where(x => x.CardName == cardName).FirstOrDefaultAsync();
			return card;
		}

		public async Task<CardType> GetCardType(int cardTypeID)
		{
			var cardType = await this.Context.CardTypes.FindAsync(cardTypeID);
			return cardType;
		}

        public async Task<List<CardType>> GetCardTypes()
        {
			return await this.Context.CardTypes.ToListAsync();
        }

        public async Task<List<Card>> GetSpellTrapCards()
		{
			var cards = await this.Context.Cards
				.Include(x => x.Effect)
				.Include(x => x.CardType)
				.Where(x => x.Discriminator == "Card" && x.ImgPath.StartsWith("Resources/Images/"))
				.ToListAsync();
			return cards;
		}
		public async Task<List<MonsterCard>> GetMonsterCards()
		{
			var cards = await this.Context.MonsterCards
				.Include(x => x.Effect)
				.Include(x=>x.CardType)
				.Where(x=>x.ImgPath.StartsWith("Resources/Images/"))
				.ToListAsync();
			return cards;
		}

		public async Task<MonsterCard> GetMonsterCard(int cardID)
		{
			var card = await this.Context.MonsterCards
				.Include(x => x.Effect)
				.Include(x => x.CardType)
				.Where(x => x.ID==cardID)
				.FirstOrDefaultAsync();
			if (card == null)
			{
				throw new Exception("There is no card with this ID");
			}
			return card;
		}

		public async Task<Card> GetCard(int cardID)
		{
			var card = await this.Context.Cards
				.Include(x => x.Effect)
				.Include(x => x.CardType)
				.Where(x => x.ID == cardID)
				.FirstOrDefaultAsync();
			if (card == null)
			{
				throw new Exception("There is no card with this ID");
			}
			return card;
		}
	}
}
