using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
    public class LPChange : IFactory
    {
        public string Description { get; set; }
		public Effect Effect { get; set; }

		public LPChange(string effectType, int pointsAddedLost)
        {
			this.Effect = new Effect();
            this.SetDescription(effectType, pointsAddedLost);
			this.SetEffect(pointsAddedLost);
        }

        public string SetDescription(string effectType,int pointsAddedLost)
        {
			switch (effectType)
			{
				case "lpchangeAdd": return this.Description = "This card restores " + pointsAddedLost + " life points to player who played this card.";
                case "lpchangeReduce": return Description = "This card reduces " + pointsAddedLost + " life points from your opponent.";
				default: throw new ArgumentException("Invalid type", "effectType");
			}
		}

		public string GetDescription()
		{
			return this.Description;
		}

		public void SetEffect(int pointsAddedLost)
		{
			this.Effect.Description = Description;
			this.Effect.PointsAddedLost = pointsAddedLost;
		}

		public Effect GetEffect()
		{
			return this.Effect;
		}
	}
}
