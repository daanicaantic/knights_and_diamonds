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
using static DAL.Repositories.Contracts.ICardRepository;
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

		public async Task<Card> AddCard(Card card)
		{
			this.Context.Cards?.Include(x => x.Effect);
			await this.Context.Cards.AddAsync(card);
			return card;
		}

		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			this.Context.MonsterCards?.Include(x => x.Effect);
			await this.Context.MonsterCards.AddAsync(card);
			return card;
		}
		public MonsterCard UpdateMonsterCard(MonsterCard card)
		{
			this.Context.MonsterCards.Update(card);
			return card;
		}

		public async Task<CardType> GetCardType(int cardTypeID)
		{
			var cardType = await this.Context.CardTypes.FindAsync(cardTypeID);
			if (cardType == null)
			{
				throw new Exception("There is no this CardType");
			}
			return cardType;
		}
		public async Task<CardType> GetCardTypeByName(string type)
		{
			var cardType = await this.Context.CardTypes?.Where(x=>x.Type==type).FirstOrDefaultAsync();
			if (cardType == null)
			{
				throw new Exception("There is no card type with this Type");
			}
			return cardType;
		}

		public async Task<List<CardType>> GetCardTypes()
        {
			return await this.Context.CardTypes?.ToListAsync();
        }

        public async Task<List<Card>> GetSpellTrapCards()
		{
			var cards = await this.Context.Cards?
				.Include(x => x.Effect)
				.Include(x => x.CardType)
				.Where(x => x.Discriminator == "Card" && x.ImgPath.StartsWith("Resources/Images/"))
				.ToListAsync();
			return cards;
		}
		public async Task<List<MonsterCard>> GetMonsterCards()
		{
			var cards = await this.Context.MonsterCards?
				.Include(x => x.Effect)
				.Include(x=>x.CardType)
				.Where(x=>x.ImgPath.StartsWith("Resources/Images/"))
				.ToListAsync();
			return cards;
		}

		public async Task<MonsterCard> GetMonsterCard(int cardID)
		{
			var card = await this.Context.MonsterCards?
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
			var card = await this.Context.Cards?
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
		public async Task<Card> GetCardAndCardsInDeck(int cardID)
		{
			var card = await this.Context.Cards?
				.Include(x=>x.CardInDecks)
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
		public async Task<FilteredCards> FilterAndOrderCards(string typleFilter,string sortOrder,string nameFilter, int pageNumber,int pageSize)
		{
			var query =this.Context?.Cards?.Include(x=>x.CardType).Include(x=>x.Effect)
				.Where(x => (string.IsNullOrEmpty(typleFilter) || x.CardType.Type == typleFilter) &&
							(string.IsNullOrEmpty(nameFilter) || x.CardName.Contains(nameFilter)) && 
							x.ImgPath.StartsWith("Resources/Images/"));
			
			if (sortOrder == "orderBy")
			{
				query = query?.OrderBy(x => x.ID);
			}
			else if (sortOrder == "orderByDesc")
			{
				query = query?.OrderByDescending(x => x.ID);
			}
			int totalItems = await query?.CountAsync();
			int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
			query=query?.Skip((pageNumber - 1) * pageSize).Take(pageSize);
			var cards = await query?.ToListAsync();
			
			var filteredCard = new FilteredCards();
			filteredCard.PageSize = pageSize;
			filteredCard.PageNumber = pageNumber;
			filteredCard.TotalItems = totalItems;
			filteredCard.TotalPages = totalPages;
			filteredCard.Cards = cards;

			return filteredCard;
		}
	}
}
