using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Deck
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		[JsonIgnore]
		public User? User { get; set; }
		[JsonIgnore]
		public virtual List<CardInDeck>? CardsInDeck { get; set; }

    }
}
