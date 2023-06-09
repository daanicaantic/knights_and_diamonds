using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public ICardService _cardService { get; set; }
        public PlayerService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _unitOfWork = new UnitOfWork(_context);
            _cardService = new CardService(_context);
        }

        public async Task<Player> GetPlayer(int playerID)
        {
            var player = await this._unitOfWork.Player.GetOne(playerID);
            if (player == null)
            {
                throw new Exception("There is no player with this ID");
            }
            return player;
            
        }

        public async Task SetGameStarted(Player player)
        {
            player.GaemeStarted = true;
            this._unitOfWork.Player.Update(player);
            await this._unitOfWork.Complete();
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
            if (player.Deck == null)
            {
				throw new Exception("Deck count is Null");
			}
			return player.Deck.Count();
        }
        
        public async Task<MappedCard> Draw(int playerID) 
        {
            var player = await this._unitOfWork.Player.GetPlayerWithHandAndDeckByID(playerID);
            if (player == null)
            {
                throw new Exception("There is no Player with this ID"); 
            }

			int numberOfCards = player.Deck.Count();
			if (numberOfCards <= 0)
			{
				Console.WriteLine("Deck count " + player.Deck.Count.ToString());
				throw new Exception("Error. There is no more cards in your deck!!");
			}
			Console.WriteLine("--------------------------------------------------------" + player.Deck.Count.ToString());
			Console.WriteLine("--------------------------------------------------------" + player.Deck.Count.ToString());
			Console.WriteLine("--------------------------------------------------------" + player.Deck.Count.ToString());

			int randomIndex = new Random().Next(0, numberOfCards - 1);
			var cardFromDeck = player.Deck[randomIndex];

			player.Deck.Remove(cardFromDeck);
            player.Hand.CardsInHand.Add(cardFromDeck);
            await this._unitOfWork.Complete();
            return await this._cardService.MapCard(cardFromDeck);

        }

        public async Task<List<MappedCard>> GetPlayersHand(int playerID) 
        {
			var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);
            if (playersHand == null)
            {
                throw new Exception("This player dont have hands :(");
            }

			var mappedHand = await this._cardService.MapCards(playersHand.CardsInHand);
			return mappedHand;
		}
		public async Task<List<CardOnFieldDisplay>> GetPlayersCardFields(int playerID)
		{
            var playerFields=await this._unitOfWork.CardField.GetFieldsByPlayerID(playerID);
			var cardsOnField = new List<CardOnFieldDisplay>();
			foreach (var field in playerFields)
			{
				var cardOnField = new CardOnFieldDisplay();
				if (field.CardOnField != null)
				{
					var mappedCard = await this._cardService.MapCard(field.CardOnField);
					cardOnField.CardOnField = mappedCard;
				}
				cardOnField.FieldID = field.ID;
				cardOnField.CardPosition = field.CardPosition;
				cardOnField.CardShowen = field.CardShowen;
				cardOnField.FieldIndex = field.FieldIndex;
				cardsOnField.Add(cardOnField);
			}
			cardsOnField = cardsOnField.OrderBy(x => x.FieldID).ToList();
			return cardsOnField;
		}

        public async Task TakeCardFromEnemiesHand(PlayersHand enemiesHand, int playerID, int cardID)
        {
            if(enemiesHand.CardsInHand == null)
            {
                throw new Exception("This player doesn't have cards in hand.");
            }

            var chosenCard = enemiesHand.CardsInHand.Where(c => c.ID == cardID).FirstOrDefault();

            if(chosenCard == null)
            {
                throw new Exception("Chosen card not found.");
            }

            var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);

            if (playersHand == null)
            {
                throw new Exception("This player doesn't have hands :(");
            }

            enemiesHand.CardsInHand.Remove((CardInDeck)chosenCard);
            playersHand.CardsInHand.Add((CardInDeck)chosenCard);

            this._unitOfWork.PlayerHand.Update(enemiesHand);
            this._unitOfWork.PlayerHand.Update(playersHand);
        }

        public async Task TakeCardFromGraveToHand(Grave grave, int playerID, int cardID)
        {
            if(grave.ListOfCardsInGrave == null)
            {
                throw new Exception("Grave doesn't have cards.");
            }

            var chosenCard = grave.ListOfCardsInGrave.Where(c => c.ID == cardID).FirstOrDefault();
            if (chosenCard == null)
            {
                throw new Exception("Chosen card not found.");
            }

            var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);
            if (playersHand == null)
            {
                throw new Exception("This player doesn't have hands :(");
            }

            playersHand.CardsInHand.Add(chosenCard);
            grave.ListOfCardsInGrave.Remove(chosenCard);

            this._unitOfWork.PlayerHand.Update(playersHand);
            this._unitOfWork.Grave.Update(grave);
        }

        public void TakeCardFromGraveToField(Grave grave, CardField cardField, int cardID)
        {
            if (grave.ListOfCardsInGrave == null)
            {
                throw new Exception("Grave doesn't have cards.");
            }

            var chosenCard = grave.ListOfCardsInGrave.Where(c => c.ID == cardID).FirstOrDefault();
            if (chosenCard == null)
            {
                throw new Exception("Chosen card not found.");
            }

            cardField.CardOnField = chosenCard;
            grave.ListOfCardsInGrave.Remove(chosenCard);

            this._unitOfWork.CardField.Update(cardField);
            this._unitOfWork.Grave.Update(grave);
        }

        public async Task SetFieldPosition(int playerID, bool position)
        {
            var listOfPlayersField = await this._unitOfWork.CardField.GetEmptyPlayerFields(playerID, "MonsterField");
            var emptyField = listOfPlayersField.FirstOrDefault();

            emptyField.CardPosition = position;
            emptyField.CardShowen = true;

            this._unitOfWork.CardField.Update(emptyField);
            await this._unitOfWork.Complete();
        }

		public async Task StartingDrawing(int playerID)
		{
			var countOfCards = 0;
			while (countOfCards < 5)
			{
				var card = await this.Draw(playerID);
				countOfCards = countOfCards + 1;
			}
		}
	}
}
