using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public enum Mode
	{
		Attack,
		Defense
	}
	public enum Face
	{
		Up,
		Down
	}
	public class Card
	{
		[Key]
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? CardType { get; set; }
		public bool CardPosition { get; set; }
		public string? ImgPath { get; set; }
		public Effect Effect{ get; set; }
	}
}
