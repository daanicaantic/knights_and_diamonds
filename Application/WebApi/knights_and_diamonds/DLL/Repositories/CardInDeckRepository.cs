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
			var card = await Context.CardInDecks.Where(x => x.CardID == cardID && x.DeckID == deckID).FirstOrDefaultAsync();
            return card;
        }
    }
}
