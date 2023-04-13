using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Strategy.Context
{
	public interface IEffectExecution
	{
		public enum ChooseCardsFrom
		{
			NoChoose,
			Field,
			GraveMonsteCard,
			GraveSTCard,
			EnemiesField,
			EnemiesHand
		}
		public ChooseCardsFrom SelectCardsFrom();
		Task ExecuteEffect(List<int> listOfIDs, string description, int playerID);
	}
}
