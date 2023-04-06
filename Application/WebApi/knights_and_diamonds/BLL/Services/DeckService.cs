using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
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
        public ICardService _cardservice { get; set; }
        public DeckService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            unitOfWork = new UnitOfWork(_context);
            _cardservice = new CardService(_context);
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
        
		public async Task<List<CardDisplayDTO>> GetCardsFromDeck(int userID)
        {
            var user = await this.unitOfWork.User.GetOne(userID);
            if(user == null)
            {
                throw new Exception("User not found.");
            }

            var mainDeckID = user.MainDeckID;

            List<CardDisplayDTO> deck = new List<CardDisplayDTO>();

            var cards = await this.unitOfWork.Deck.GetCardsFromDeck(mainDeckID, userID);
            if (cards == null)
            {
                throw new Exception("This user doesn't have main deck.");
            }

            foreach (var card in cards)
            {
                var mappedCard = await _cardservice.MapCard(card);
                deck.Add(mappedCard);
            }
            return deck;
        }
    }
}
