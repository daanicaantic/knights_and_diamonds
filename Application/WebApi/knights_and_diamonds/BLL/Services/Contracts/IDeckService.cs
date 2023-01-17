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
        void AddDeck(Deck deck);
        void AddCardToDeck(int cardID, int deckID);
        public Task<IList<CardInDeck>> GetCards(int id);




	}
}
