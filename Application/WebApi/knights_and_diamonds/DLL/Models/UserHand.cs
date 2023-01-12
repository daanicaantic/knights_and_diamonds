using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class UserHand
	{
		public int ID { get; set; }
		public User User { get; set; }
		public Deck Deck { get; set; }
		public List<Card> Cards { get; set; }
	}
}
