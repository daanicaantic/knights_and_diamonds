using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
	public struct CardOnFieldDisplay
	{
		public MappedCard CardOnField { get; set; }
		public int FieldID { get; set; }
		public int FieldIndex { get; set; }
		public bool CardPosition { get; set; } //0-napad,1-odbrana
		public bool CardShowen { get; set; }

	}
	public class FieldDTO
    {
        public int LifePoints { get; set; }
		public bool GameStarted { get; set; }
        public int DeckCount { get; set; }
		public List<MappedCard> Hand { get; set; }
		public List<CardOnFieldDisplay> CardFields { get; set; }

	}
	public class EnemiesFieldDTO
	{
		public int LifePoints { get; set; }
		public int DeckCount { get; set; }
		public int HandCount { get; set; }
		public List<CardOnFieldDisplay> CardFields { get; set; }

	}
}
