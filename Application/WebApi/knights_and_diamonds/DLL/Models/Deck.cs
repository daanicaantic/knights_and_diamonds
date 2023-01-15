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
		public List<Card>? ListOfCards { get; set; }
        public Deck()
        {
            this.ListOfCards = new List<Card>();
        }
    }
}
