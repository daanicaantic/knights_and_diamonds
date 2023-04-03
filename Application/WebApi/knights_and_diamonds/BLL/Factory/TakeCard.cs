using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
	public class TakeCard : IFactory
	{
		public string Description { get; set; }
		public Effect Effect { get; set; }
		public TakeCard(string effectType, int numOFCardAffected)
		{
			this.Effect = new Effect();
			this.SetDescription(effectType, numOFCardAffected);
			this.SetEffect(numOFCardAffected);
		}

		public string SetDescription(string effectType, int numOFCardAffected)
		{
			string Plural = "card";
			if (numOFCardAffected > 1)
			{
				Plural = "cards";
			}

			switch (effectType)
			{
				case "takeCardFromEnemiesHand": return this.Description = "This card takes " + numOFCardAffected.ToString() + " " + Plural + " from enemies hand.";
				case "takeCardFromEnemiesField": return this.Description = "This card takes " + numOFCardAffected.ToString() + " " + Plural + " from enemies field.";

				default: throw new ArgumentException("Invalid type", effectType);
			}
		}
		public string GetDescription()
		{
			return this.Description;
		}
		public void SetEffect(int numOFCardAffected)
		{
			this.Effect.NumOfCardsAffected = numOFCardAffected;
			this.Description = this.Description;
		}
		public Effect GetEffect()
		{
			return this.Effect;
		}
	}
}
