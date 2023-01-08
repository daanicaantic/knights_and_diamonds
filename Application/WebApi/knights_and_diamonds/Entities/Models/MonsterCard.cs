using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	enum MonsterType 
	{
		SpellCater,
		Dragon,
		Cyborg,
		Knight
	}
	public class MonsterCard : Card
	{
		public int NumberOfStars { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public string? MonsterType { get; set; }

	}
}
