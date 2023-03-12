using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory.Contract
{
	public interface IDescriptionFactory
	{
		IFactory FactoryMethod(string type,string effectType,int cardsAffected,int pointsAddedLost);
	}
}
