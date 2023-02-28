using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
	public class MCardDTO
	{
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Description { get; set; }
		public int NumberOfStars { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public int MonsterType { get; set; }
		public int CardType { get; set; }
		public int ElementType { get; set; }
	}

	public class STCardDTO 
	{
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Description { get; set; }
		public int CardType { get; set; }
	}
}
