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
    public class PlayerService : IPlayerService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public PlayerService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        public async Task<List<CardInDeck>> SetPlayersDeck(int userID)
        {
            var user = await this._unitOfWork.User.GetOne(userID);
            var deck = await this._unitOfWork.Deck.GetCardsFromDeck(user.MainDeckID, userID);
            return deck;
        }
        public async Task<int> GetNumberOfCardsInDeck(int playerID)
        {
            var player = await this._unitOfWork.Player.GetPlayerByID(playerID);
			if (player == null)
			{
				throw new Exception("Player dosent exsist!");
			}
			return player.Deck.Count();
        }

        public async Task<Card> Draw(int playerID) 
        {
            var player = await this._unitOfWork.Player.GetPlayersHandByPlayerID(playerID);
            var cardFromDeck = player.Draw();
            var card = await this._unitOfWork.Card.GetOne(cardFromDeck.CardID);
            player.Deck.Remove(cardFromDeck);
            player.Hand.CardsInHand.Add(cardFromDeck);
            this._unitOfWork.Complete();
            return card;
        }

        public async Task<List<Card>> GetPlayersHand(int playerID) 
        {
            List<Card> hand = new List<Card>();
            var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);
            foreach (var card in playersHand.CardsInHand)
            {
                var c = await this._unitOfWork.Card.GetOne(card.CardID);
                hand.Add(c);
            }
            return hand;
		}
    }
}
