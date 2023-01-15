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
    public class DeckRepository : Repository<Deck>, IDeckRepository
    {
        public DeckRepository(KnightsAndDiamondsContext context) : base(context)
        {
        }
        public void AddCardToDeck(Card card, Deck d)
        {
            d.ListOfCards.Add(card);
        }
        public KnightsAndDiamondsContext Context
        {
            get { return Context as KnightsAndDiamondsContext; }
        }
    }
}
