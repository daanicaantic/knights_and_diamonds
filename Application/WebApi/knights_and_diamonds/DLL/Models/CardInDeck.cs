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
		public int CardId { get; set; }
		public Card? Card { get; set; }

		public Deck? Deck { get; set; }

	}
}
