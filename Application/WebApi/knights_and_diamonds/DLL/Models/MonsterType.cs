using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class MonsterType
	{
		public int ID { get; set; }
		[NotNull]
		public string? Type { get; set; }

		[JsonIgnore]
		public List<MonsterCard>? MonsterCards { get; set; }
	}
}
