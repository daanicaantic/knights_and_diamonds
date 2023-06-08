using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class CardInDeckRepository : Repository<CardInDeck>, ICardInDeckRepository
	{
		public CardInDeckRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

        public async Task<CardInDeck> RemoveCardFromDeck(int cardID, int deckID)
        {
			var card = await Context.CardInDecks?.Where(x => x.ID == cardID && x.DeckID == deckID).Include(x => x.Deck).Include(x => x.Grave).Include(x => x.Player).Include(x=>x.PlayersHand).Include(x=>x.CardFields).FirstOrDefaultAsync();
			if (card == null)
			{
				throw new Exception("There is some error");
			}
            return card;
        }

		public async Task<CardInDeck> GetCardInDeckWithCard(int cardID)
		{
			var card = await this.Context.CardInDecks
				.Include(x => x.Card)
				.ThenInclude(x=>x.CardType)
				.Where(x => x.ID == cardID)
				.FirstOrDefaultAsync();
			return card;
		}
	}
}
