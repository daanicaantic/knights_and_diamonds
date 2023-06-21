using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory
{
	public class DestroysCard : IFactory
	{
		public string Description { get; set; }
		public Effect Effect { get; set; }

		public DestroysCard(string effectType, int numOFCardAffected)
		{
			this.Effect = new Effect();
			this.Description=this.SetDescription(effectType, numOFCardAffected);
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
				case "destroyesMonsterAfterItIsSummoned": return this.Description = "This "+Plural+" destroys enemies monster after it is summoned.";
					
				default: throw new ArgumentException("Invalid type", effectType);
			}
		}
		public string GetDescription()
		{
			return this.Description;
		}
		public void SetEffect(int numOFCardAffected)
		{
			this.Effect.Description = this.Description;
			this.Effect.NumOfCardsAffected = numOFCardAffected;
		}
		public Effect GetEffect()
		{
			return this.Effect;
		}
}
}
