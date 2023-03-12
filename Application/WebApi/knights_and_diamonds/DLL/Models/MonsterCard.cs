using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class MonsterCard:Card
	{
		public int NumberOfStars { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public int MonsterTypeID { get; set; }
		[JsonIgnore]
		public MonsterType? MonsterType { get; set; }
		public int ElementTypeID { get; set; }
		[JsonIgnore]
		public ElementType? ElementType { get; set; }
		public MonsterCard() 
		{
		
		}
		public MonsterCard(string? cardName, string? imgPath, int numberOfStars, int attackPoints, int defencePoints,Effect effect, int monsterTypeID, int cardTypeID, int elementTypeID):base(cardName,imgPath, cardTypeID,cardTypeID,effect)
		{
			NumberOfStars = numberOfStars;
			AttackPoints = attackPoints;
			DefencePoints = defencePoints;
			MonsterTypeID = monsterTypeID;
			ElementTypeID = elementTypeID;
		}
	}
}
