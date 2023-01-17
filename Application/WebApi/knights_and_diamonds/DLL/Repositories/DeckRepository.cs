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


        public async Task<IList<CardInDeck>> GetCardsFromDeck(int DeckId)
        {
            try
            {
                return await this.Context.CardInDecks.Where(x => x.Deck.ID == DeckId)
                    .Include(x => x.Card)
                    .Include(x => x.Deck)
                    .ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
