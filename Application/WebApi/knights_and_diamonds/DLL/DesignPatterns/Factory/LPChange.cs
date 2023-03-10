using DAL.DesignPatterns.Factory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
    public class LPChange : IFactory
    {
        public string Description { get; set; }
        public LPChange(string effectType,int pointsAddedLost)
        {
            this.SetDescription(effectType, pointsAddedLost);
        }

        public string SetDescription(string effectType,int pointsAddedLost)
        {
			switch (effectType)
			{
				case "lpchangeAdd":return this.Description = "This card restores " + pointsAddedLost + " life points to player who played this card";
                case "lpchangeReduce": return Description = "This card reduces " + pointsAddedLost + " life points to your oponent";
				default: throw new ArgumentException("Invalid type", "effectType");
			}
		}

		public async Task<string> GetDescription()
		{
			return this.Description;
		}
	}
}
