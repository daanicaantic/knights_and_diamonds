using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public enum Play
	{
		Rock,
		Paper,
		Scissors
	}
	public class Player
	{
		[Key]
		public int ID { get; set; }
		public Play? Play { get; set; }
		public int RPSGameID { get; set; }
		public RockPaperScissorsGame? RPSGame { get; set; }
		public int LifePoints { get; set; }
		public int UserID { get; set; }
        public User? User { get; set; }
		public int? GameID { get; set; }
		public Game? Game { get; set; }
		public List<CardInDeck>? Deck { get; set; }
		public PlayersHand? Hand { get; set; }

		public Player() 
		{ 
		
		}

		public Player(RockPaperScissorsGame? rPSGame, Game game, User? user, List<CardInDeck> deck)
		{
			this.RPSGameID = rPSGame.ID;
			this.RPSGame = rPSGame;
			this.UserID = user.ID;
			this.User = user;
			this.Game = game;
			this.Deck = deck;
			this.Hand = new PlayersHand();
			this.LifePoints = 8000;
		}
		public CardInDeck Draw()
		{
			if (this.Deck.Count <= 0) 
			{
				throw new Exception("Error. There is no more cards in your deck!!");
			}
			int numberOfCards = this.Deck.Count - 1;
			int randomIndex = new Random().Next(0, numberOfCards);
			var card = this.Deck[randomIndex];
			return card;
		}
	}
}
