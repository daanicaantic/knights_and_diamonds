
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
		public int? EffectID { get; set; }
		[JsonIgnore]
		public Effect? Effect { get; set; }
		public int CardTypeID { get; set; }
		[JsonIgnore]
		public CardType? CardType { get; set; }
		
		[JsonIgnore]
		public virtual List<CardInDeck>? CardInDecks { get; set; }

		public Card()
		{

		}

		public Card(string? cardName, string? imgPath,int cardTypeID,int effectID,Effect effect) 
		{
			CardName = cardName;
			ImgPath = imgPath;
			CardTypeID = cardTypeID;
			EffectID = effectID;
			Effect = effect;
			
		}
	}
}
