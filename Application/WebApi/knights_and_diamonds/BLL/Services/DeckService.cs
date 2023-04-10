using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Migrations;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public struct CardCounter
    {
        public int AllCardsCount { get; set; }
        public int MonsterCardsCount { get; set; }
        public int TrapCardsCount { get; set; }
        public int SpellCardsCount { get; set; }
    }

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

        public async Task<User> SetMainDeckID(int userID, int deckID)
        {

            var deck = await this.unitOfWork.Deck.GetCardsFromDeck(userID, deckID);
            if (deck == null)
            {
                throw new Exception("This user doesn't contains deck with this ID");
            }
            var user = await this.unitOfWork.Deck.SetMainDeck(userID, deckID);
            await this.unitOfWork.Complete();
            return user;
        }
        
		public async Task<List<MappedCard>> GetCardsFromDeck(int userID)
        {
            var user = await this.unitOfWork.User.GetOne(userID);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var mainDeckID = user.MainDeckID;

            var cards = await this.unitOfWork.Deck.GetCardsFromDeck(mainDeckID, userID);
            if (cards == null)
            {
                throw new Exception("This user doesn't have main deck.");
            }

            var deck = await this._cardservice.MapCards(cards);
            cards = cards.OrderBy(x => x.CardID).ToList();
            cards = cards.OrderBy(x => x.Card.CardTypeID).ToList();

            foreach (var card in cards)
            {
                var mappedCard = await _cardservice.MapCard(card);
                deck.Add(mappedCard);
            }

            return deck;
        }

        public async Task AddCardToDeck(int cardID, int deckID)
        {
            CardInDeck cardInDeck = new CardInDeck();
            var c = await this.unitOfWork.Card.GetOne(cardID);
            if (c == null) 
            {
                throw new Exception("There is no card with this ID");
            }
            var d = await this.unitOfWork.Deck.GetOne(deckID);
			if (d == null)
			{
				throw new Exception("There is no deck with this ID");
			}
            var userID = d.UserID;
            var cInD = await this.unitOfWork.Deck.GetCardsFromDeck(deckID, userID);
            if (cInD.Count == 40)
            {
                throw new Exception("You have reached card limit.");
            }
			cardInDeck.Card = c;
            cardInDeck.Deck = d;
            await this.unitOfWork.CardInDeck.Add(cardInDeck);
            await this.unitOfWork.Complete();
        }

        public async Task RemoveCardFromDeck(int cardID, int deckID)
        {
            var cardInDeck = await this.unitOfWork.CardInDeck.RemoveCardFromDeck(cardID, deckID);
            if (cardInDeck == null)
            {
                throw new Exception("There is no card in deck with this ID");
            }
            this.unitOfWork.CardInDeck.Delete(cardInDeck);
            await this.unitOfWork.Complete();
        }

        public async Task<CardCounter> CardCounter(int deckID, int userID)
        {
            var cardCounter = new CardCounter();

            var deck = await this.unitOfWork.Deck.GetCardsFromDeck(deckID, userID);
            if (deck == null)
            {
                throw new Exception("There is no deck with this ID");
            }

            cardCounter.AllCardsCount =  deck.Count();

            foreach (var card in deck)
            {
                var cardTypeID = await this.unitOfWork.Card.GetCardType(card.Card.CardTypeID);
                if (cardTypeID.Type == "MonsterCard")
                {
                    cardCounter.MonsterCardsCount += 1;
                }
                else if (cardTypeID.Type == "SpellCard")
                {
                    cardCounter.SpellCardsCount += 1;
                }
                else if (cardTypeID.Type == "TrapCard")
                {
                    cardCounter.TrapCardsCount += 1;
                }
            }

            return cardCounter;
        }
    }
}
