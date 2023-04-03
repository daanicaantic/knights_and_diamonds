using BLL.Factory;
using DAL.DesignPatterns.Factory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
	public class ConcreteEffectFactory : IEffectFactory
	{
		public IFactory FactoryMethod(string type, string effectType, int cardsAffected, int pointsAddedLost)
		{
			switch (type)
			{
				case "draw": return new DrawCard(cardsAffected);
				case "lpchange": return new LPChange(effectType, pointsAddedLost);
				case "return": return new ReturnCard(effectType, cardsAffected);
				case "take": return new TakeCard(effectType, cardsAffected);
				default: return new NoEffect();
			}
		}
	}
}
