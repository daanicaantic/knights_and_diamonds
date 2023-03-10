using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
    public interface IDeckRepository : IRepository<Deck>
    {
		Task<Deck> AddDeck(Deck deck);
		Task<IList<Card>> GetCardsFromDeck(int DeckId);
	}
}
