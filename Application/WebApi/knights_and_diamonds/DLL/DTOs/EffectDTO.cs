using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTOs
{
	public class EffectDTO
	{
		public int EffectTypeID { get; set; }
		public int? NumOfCardsAffected { get; set; }
		public int? PointsAddedLost { get; set; }
	}
}
