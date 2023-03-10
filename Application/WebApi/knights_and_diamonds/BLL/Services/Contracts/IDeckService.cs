using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IDeckService
    {
        Task<Deck> AddDeck(Deck deck);

        Task AddCardToDeck(int cardID, int deckID);
        Task<IList<Card>> ShuffleDeck(int DeckID);

		public Task<IList<Card>> GetCards(int id);
	}
}
