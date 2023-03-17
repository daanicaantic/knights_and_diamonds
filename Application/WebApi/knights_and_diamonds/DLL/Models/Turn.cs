using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Turn
	{
		[Key]
		public int ID { get; set; }
		public bool DrawPhase { get; set; }
		public bool MainPhase { get; set; }
		public bool BattlePhase { get; set; }
		public bool EndPhase { get; set; }
		public bool MonsterSummoned { get; set; }
		public int TurnNumber { get; set; }
		[JsonIgnore]
		public int PlayerOnTurn { get; set; }
		public Game? Game { get; set; }

		public Turn()
		{
			this.DrawPhase = true;
			this.MainPhase = false;
			this.BattlePhase = false;
			this.MonsterSummoned = false;
			this.EndPhase = false;
		}
	}
}
