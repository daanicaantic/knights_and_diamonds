using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Card
	{
		[Key]
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Description { get; set; }
		public int NumberOfStars { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public MonsterType? MonsterType { get; set; }
		public CardType? CardType { get; set; }
        public ElementType? ElementType { get; set; }

		[JsonIgnore]
		public virtual List<CardInDeck>? CardInDecks { get; set; }


	}
}
