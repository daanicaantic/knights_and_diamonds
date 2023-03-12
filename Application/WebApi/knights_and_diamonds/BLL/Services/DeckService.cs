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
            var d=await this.unitOfWork.Deck.AddDeck(deck);
            this.unitOfWork.Complete();
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
            this.unitOfWork.CardInDeck.Add(cardInDeck);
            this.unitOfWork.Complete();
        }
        public async Task<IList<Card>> ShuffleDeck(int DeckID)
        {
            var deck = await this.unitOfWork.Deck.GetCardsFromDeck(DeckID);
       
            int lastIndex = deck.Count() - 1;
            while (lastIndex > 0)
            {
                var tempValue = deck[lastIndex];
                int randomIndex = new Random().Next(0, lastIndex);
                deck[lastIndex] = deck[randomIndex];
                deck[randomIndex] = tempValue;
                lastIndex--;
            }
            return deck;
		}
		public async Task<Card> DrawCard(IList<Card> cards)
		{
            if (cards.Count <= 0) 
            {
                throw new Exception("There is not card in your deck anymore, you lost!!");
            }
            var card = cards.FirstOrDefault();
			return card;
		}
        
		public async Task<IList<Card>> GetCards(int DeckID)
        {
            return await this.unitOfWork.Deck.GetCardsFromDeck(DeckID);
        }

  
    }
}
