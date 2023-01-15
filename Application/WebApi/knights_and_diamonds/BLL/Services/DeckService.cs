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
            Card c=this.unitOfWork.Card.GetOne(cardID);
            Deck d=this.unitOfWork.Deck.GetOne(deckID);
            this.unitOfWork.Deck.AddCardToDeck(c, d);
         
            this.unitOfWork.Complete();
        }

        public List<Card> GetDeck(int id)
        {
            try
            {
                Deck d = this.unitOfWork.Deck.GetOne(id);

                List<Card> cards = new List<Card>();
                foreach (Card c in d.ListOfCards)
                {
                    cards.Add(c);
                }
                return cards;
            }
            catch
            {
                throw;
            }
        }
    }
}
