using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class CardInDeck
	{
		[Key]
		public int ID { get; set; }
		public int CardID { get; set; }
		[JsonIgnore]
		public Card? Card { get; set; }
		public int DeckID { get; set; }
		[JsonIgnore]
		public Deck? Deck { get; set; }
		[JsonIgnore]
		public Player? Player { get; set; }
		[JsonIgnore]
		public PlayersHand? PlayersHand { get; set; }
		[JsonIgnore]
		public Grave? Grave { get; set; }
		public List<CardField>? CardFields { get; set; }

	}
}
