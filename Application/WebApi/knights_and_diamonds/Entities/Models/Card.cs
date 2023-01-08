using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class Card
	{
		[Key]
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Effect { get; set; }

	}
}
