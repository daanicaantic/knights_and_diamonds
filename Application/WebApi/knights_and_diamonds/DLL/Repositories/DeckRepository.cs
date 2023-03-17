using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DeckRepository : Repository<Deck>, IDeckRepository
    {
        public DeckRepository(KnightsAndDiamondsContext context) : base(context)
        {

        }
        public KnightsAndDiamondsContext Context
        {
            get { return _context as KnightsAndDiamondsContext; }
        }

        public async Task<Deck> AddDeck(Deck deck)
        {
            var user = await this.Context.Users.FindAsync(deck.UserID);
            this.Context.Decks.Include(x => x.User);
            deck.User = user;
            this.Context.Add(deck);
            return deck;
        }
      /*  public async Task<CardInDeck> addCardInDeck(int cardID, int deckID) 
        {
            this.Context.CardInDecks.Include(x => x.Card).Include(x => x.Deck);
            await this.Context.Cards.FindAsync(cardID);
			await this.Context.Cards.FindAsync(deckID);

		}*/
		public async Task<List<CardInDeck>> GetCardsFromDeck(int deckID, int userID)
        {
            var deck = await Context.CardInDecks.Where(x => x.Deck.ID == deckID && x.Deck.UserID == userID)
                .Include(x => x.Card)
                .Include(x => x.Deck)
                .ToListAsync();
            return deck;
        }
    }
}
