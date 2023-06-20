using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class AttackInTurn
	{
		public int ID { get; set; }
		public bool IsAttackingAbble { get; set; }
		public int TurnID { get; set; }
		[JsonIgnore]
		public Turn? Turn { get; set; }
		public int CardFieldID { get; set; }
		[JsonIgnore]
		public CardField? CardField { get; set; }
		public AttackInTurn()
		{

		}
		public AttackInTurn(bool isAttackingAbble,int turnID,int cardFieldID)
		{
			this.IsAttackingAbble = isAttackingAbble;
			this.TurnID = turnID;
			this.CardFieldID = cardFieldID;
		}
	}
}
