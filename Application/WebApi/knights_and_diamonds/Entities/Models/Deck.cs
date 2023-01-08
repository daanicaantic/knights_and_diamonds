using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class Deck
	{
		public int ID { get; set; }
		public List<Card> ListOfCards { get; set; }
	}
}
