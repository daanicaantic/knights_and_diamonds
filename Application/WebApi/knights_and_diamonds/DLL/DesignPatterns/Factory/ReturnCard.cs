using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
	public class ReturnCard : IFactory
	{
		public string Description { get; set; }
		
		public ReturnCard(string effectType, int numOFCardAffected)
		{
			this.SetDescription(effectType, numOFCardAffected);
		}
	
		public string SetDescription(string effectType, int numOFCardAffected)
		{
			string	Plural = "card";
			if (numOFCardAffected > 1) 
			{
				Plural = "cards";
			}
			switch (effectType)
			{
				case "returnCardFromFieldToHand": return this.Description = "This card returns" + numOFCardAffected.ToString() + " " + Plural + " from field to players hand"; ;
				case "returnStFromGraveToHand": return this.Description = "This card returns" + numOFCardAffected.ToString() + "spell or trap " + Plural + " from grave to your hand";
				case "returnMonsterFromGraveToHand": return this.Description = "This card returns" + numOFCardAffected.ToString() + "monster " + Plural + " from grave to your hand";
				case "retunrMonsterFromGraveToField": return this.Description = "This card returns" + numOFCardAffected.ToString() + "monster " + Plural + " from grave to the field";

				default: throw new ArgumentException("Invalid type", "effectType");
			}
		}
		public async Task<string> GetDescription()
		{
			return this.Description;
		}
	}
}
