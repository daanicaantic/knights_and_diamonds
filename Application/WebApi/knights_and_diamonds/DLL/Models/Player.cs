using System;
using System.Collections.Generic;
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
		public int UserID { get; set; }
        public User? User { get; set; }
        public Game? Game { get; set; }

        public Player() 
		{ 
		
		}
		public Player(RockPaperScissorsGame? rPSGame, User? user)
		{
			RPSGameID = rPSGame.ID;
			RPSGame = rPSGame;
			UserID = user.ID;
			User = user;
		}
	}
}
