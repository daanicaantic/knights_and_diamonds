using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DeckService : IDeckService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork unitOfWork { get; set; }
        public DeckService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            unitOfWork = new UnitOfWork(_context);
        }
        public void AddDeck(Deck deck)
        {
            try
            {
                this.unitOfWork.Deck.Add(deck);
                this.unitOfWork.Complete();
            }
            catch
            {
                throw;
            }
        }
        public void AddCardToDeck(int cardID, int deckID)
        {
        /*    CardInDeck cardInDeck = new CardInDeck();
            var c = this.unitOfWork.Card.GetOne(cardID);
            var d = this.unitOfWork.Deck.GetOne(deckID);
            cardInDeck.Card = c;
            cardInDeck.Deck = d;
            this.unitOfWork.CardInDeck.Add(cardInDeck);
            this.unitOfWork.Complete();*/
        }
        public async Task<IList<CardInDeck>> GetCards(int id) 
        {
            try
            {
                return await this.unitOfWork.Deck.GetCardsFromDeck(id);
            }
            catch 
            {
                throw;
            }
        }

  
    }
}
