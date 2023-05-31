using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.Win32.SafeHandles;
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
        public ICardService _cardservice { get; set; }
        public PlayerService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _unitOfWork = new UnitOfWork(_context);
            _cardservice = new CardService(_context);
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
            return await this._cardservice.MapCard(cardFromDeck);

        }

        public async Task<List<MappedCard>> GetPlayersHand(int playerID) 
        {
			var playersHand = await this._unitOfWork.Player.GetPlayersHand(playerID);
            if (playersHand == null)
            {
                throw new Exception("This player dont have hands :(");
            }
			var mappedHand = await this._cardservice.MapCards(playersHand.CardsInHand);
			return mappedHand;
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
