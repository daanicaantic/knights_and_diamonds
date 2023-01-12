using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public enum MonsterTP
	{
		spellcaster,
		dragon,
		knight
	}
	public class Card
	{
		[Key]
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Effect { get; set; }
		public int NumberOfStars { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public string? MonsterType { get; set; }
		public string? cardtype { get; set; }

	}
}
