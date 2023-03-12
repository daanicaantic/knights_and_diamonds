using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Effect
	{
		public int ID { get; set; }
		public int? EffectTypeID { get; set; }
		[JsonIgnore]
		public EffectType? EffectType { get; set; }
		public string? Description { get; set; }
		public int? NumOfCardsAffected { get; set; }
		public int? PointsAddedLost { get; set; }
		[JsonIgnore]
		public List<Card> Cards { get; set; }
		public Effect()
		{

		}
		public Effect(int effectTypeID, EffectType? effectType, string? description, int? numOfCardsAffected, int? pointsAddedLost)
		{
			EffectTypeID = effectTypeID;
			EffectType = effectType;
			Description = description;
			NumOfCardsAffected = numOfCardsAffected;
			PointsAddedLost = pointsAddedLost;
		}
	}
}
