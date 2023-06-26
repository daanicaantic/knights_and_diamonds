using DAL.Models;
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
			GraveMonsterCard,
			GraveSTCard,
			EnemiesField,
			EnemiesHand
		}
		public ChooseCardsFrom SelectCardsFrom();
		Task ExecuteEffect(List<int> listOfIDs, Effect effect, int playerID,int gameID,int fieldID);
		string WhenCanYouActivateTrapCard();
	}
}
