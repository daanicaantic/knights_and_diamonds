using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ElementType
	{
		public int ID { get; set; }
		[NotNull]
		public string? Type { get; set; }
		public string? ImgPath { get; set; }
		[JsonIgnore]
		public List<Card>? Cards { get; set; }
	}
}
