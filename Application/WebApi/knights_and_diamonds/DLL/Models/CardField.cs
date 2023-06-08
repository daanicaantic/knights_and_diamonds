using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class CardField
	{
		public int ID { get; set; }
		public int? CardOnFieldID { get; set; }
		[JsonIgnore]
		public CardInDeck? CardOnField { get; set; }
		public int FieldIndex { get; set; }
		public string? FieldType { get; set; }
		public bool CardPosition { get; set; } //0-napad,1-odbrana
		public bool CardShowen { get; set; } //0-otkrivena,1-neotkrivena
		public int PlayerID { get; set; }
		[JsonIgnore]
		public Player Player { get; set; }
		[JsonIgnore]
		public List<AttackInTurn> AttackInTurn { get; set; }


		public CardField()
		{

		}
		public CardField(int fieldIndex, int playerID, string fieldType )
		{
			FieldIndex = fieldIndex;
			PlayerID = playerID;
			FieldType = fieldType;
			CardPosition = true;
			CardShowen = true;
		}
	}
}
