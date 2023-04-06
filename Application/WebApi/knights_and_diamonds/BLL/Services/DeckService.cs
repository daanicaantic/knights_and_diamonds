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
        public async Task<Deck> AddDeck(Deck deck)
        {
            var d = await this.unitOfWork.Deck.AddDeck(deck);
            var user = await this.unitOfWork.User.GetOne(deck.UserID);
            if (user.MainDeckID == 0)
            {
                user.MainDeckID = d.ID;
                this.unitOfWork.User.Update(user);
            }
            await this.unitOfWork.Complete();
            return d;
        }
        public async Task AddCardToDeck(int cardID, int deckID)
        {
            CardInDeck cardInDeck = new CardInDeck();
            var c = await this.unitOfWork.Card.GetOne(cardID);
            if (c == null) 
            {
                throw new Exception("There is not card with this ID");
            }
            var d = await this.unitOfWork.Deck.GetOne(deckID);
			if (c == null)
			{
				throw new Exception("There is not deck with this ID");
			}
			cardInDeck.Card = c;
            cardInDeck.Deck = d;
            await this.unitOfWork.CardInDeck.Add(cardInDeck);
            await this.unitOfWork.Complete();
        }
        
		public async Task<List<CardInDeck>> GetCards(int deckID, int userID)
        {
            return await this.unitOfWork.Deck.GetCardsFromDeck(deckID, userID);
        }

  
    }
}
