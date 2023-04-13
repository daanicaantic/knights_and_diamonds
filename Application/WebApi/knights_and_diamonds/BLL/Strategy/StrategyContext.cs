using BLL.Strategy.Context;
using DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Strategy
{
	public class StrategyContext
	{
		private IEffectExecution _effectExecution;
		//Constructor: assigns strategy to interface  
		public StrategyContext(IEffectExecution strategy)
		{
			this._effectExecution = strategy;
		}

		//Executes the strategy  
		public int GetAreaOfSelectingCards()
		{
			return (int)this._effectExecution.SelectCardsFrom();
		}
		public async Task ExecuteEffect(List<int> listOfCards, string description, int playerID)
		{
			await this._effectExecution.ExecuteEffect(listOfCards, description, playerID);
		}
	}
}
