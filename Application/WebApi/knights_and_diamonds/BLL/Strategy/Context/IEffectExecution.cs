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
		public struct GameChanges
		{
			public bool IsYourLifePointsChanged { get; set; }
			public bool IsEnemiesLifePointsChanged { get; set; }
			public bool IsYourDeckCountChanged { get; set; }
			public bool IsEnemiesDeckCountChanged { get; set; }
			public bool IsYourHandChanged { get; set; }
			public bool IsEnemiesHandChanged { get; set; }
			public bool IsEnemiesFieldChanged { get; set; }
			public int LpChange { get; set; }
			public GameChanges(bool IsYourLifePointsChanged, bool IsEnemiesLifePointsChanged, bool IsYourDeckCountChanged, bool IsEnemiesDeckCountChanged, bool IsYourHandChanged, bool IsEnemiesHandChanged, bool IsEnemiesFieldChanged, int LpChange)
			{
				this.IsYourLifePointsChanged = IsYourLifePointsChanged;
				this.IsEnemiesLifePointsChanged = IsEnemiesLifePointsChanged;
				this.IsYourDeckCountChanged=IsYourDeckCountChanged;
				this.IsEnemiesDeckCountChanged = IsEnemiesDeckCountChanged;
				this.IsYourHandChanged = IsYourHandChanged;
				this.IsEnemiesHandChanged=IsEnemiesHandChanged;
				this.IsEnemiesFieldChanged = IsEnemiesFieldChanged;
				this.LpChange = LpChange;
			}
		}
		public ChooseCardsFrom SelectCardsFrom();
		Task ExecuteEffect(List<int> listOfIDs, Effect effect, int playerID,int gameID,int fieldID);
		string WhenCanYouActivateTrapCard();
	}
}
