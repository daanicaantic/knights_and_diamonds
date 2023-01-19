using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Connection
	{
		[Key]
		public int ID { get; set; }
		public int UserID { get; set; }
		public DateTime isStillLogeniIn { get; set; } 
	}
}
