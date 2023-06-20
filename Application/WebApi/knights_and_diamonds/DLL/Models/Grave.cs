using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Grave
	{
		[Key]
		public int ID { get; set; }
		public List<CardInDeck> ListOfCardsInGrave { get; set; }
		public Grave()
		{
			this.ListOfCardsInGrave = new List<CardInDeck>();
		}
	}
}
