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

        public async Task<CardDisplayDTO> MapCard(CardInDeck cardInDeck)
        {
            var mappedCard = new CardDisplayDTO();
			if (cardInDeck.Card.Discriminator == "Card")
			{
				var card = await this._unitOfWork.Card.GetCard(cardInDeck.CardID);
				mappedCard = new CardDisplayDTO(card.ID, card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID, card.ImgPath, card.Effect.Description);
			}
			else
			{
				var card = await this._unitOfWork.Card.GetMonsterCard(cardInDeck.CardID);
				mappedCard = new CardDisplayDTO(card.ID, card.CardName, card.CardType.Type, card.Effect.NumOfCardsAffected, card.Effect.PointsAddedLost, card.EffectID, card.NumberOfStars, card.AttackPoints, card.DefencePoints, card.ImgPath, card.Effect.Description);
			}
            return mappedCard;
		}
        public async Task<CardDisplayDTO> Draw(int playerID) 
        {
            var mappedCard=new CardDisplayDTO();
            var player = await this._unitOfWork.Player.GetPlayerWithHandAndDeckByID(playerID);
            if (player == null)
            {
                throw new Exception("There is no Player with this ID"); 
            }
            var cardFromDeck = player.Draw();
			mappedCard = await this.MapCard(cardFromDeck);
            player.Deck.Remove(cardFromDeck);
            player.Hand.CardsInHand.Add(cardFromDeck);
            this._unitOfWork.Complete();
            return mappedCard;
        }

        public async Task<List<CardDisplayDTO>> GetPlayersHand(int playerID) 
        {
            List<CardDisplayDTO> hand = new List<CardDisplayDTO>();
            var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);
            foreach (var card in playersHand.CardsInHand)
            {
                var mappedCard = await this.MapCard(card);
                hand.Add(mappedCard);
            }
            return hand;
		}
    }
}
