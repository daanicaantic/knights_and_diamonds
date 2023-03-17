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
		[JsonIgnore]
		public List<Card>? Deck { get; set; }
		[JsonIgnore]
		public PlayerHand? Hand { get; set; }


		public Player() 
		{ 
		
		}
		public Player(RockPaperScissorsGame? rPSGame,Game game, User? user)
		{
			this.RPSGameID = rPSGame.ID;
			this.RPSGame = rPSGame;
			this.UserID = user.ID;
			this.User = user;
			this.Game = game;
			this.Deck = new List<Card>();
			this.Hand = new PlayerHand();
			this.LifePoints = 8000;
		}
		public Card Draw() 
		{
			if (this.Deck.Count <= 0) {
				throw new Exception("Thers is no more cards in your dack!!");
			}
			var card = this.Deck.FirstOrDefault();
			this.Deck.Remove(card);
			return card;
		}
	}
}
