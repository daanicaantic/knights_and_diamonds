using BLL.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.DesignPatterns.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.UnitOfWork;
using BLL.Strategy.Context;

namespace BLL.Strategy
{
	public class ConcreteStrategy
	{
		private readonly KnightsAndDiamondsContext _context;
		public StrategyContext _strategyContext { get; set; }

		public ConcreteStrategy(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}

		public StrategyContext SetStrategyContext(string effectType)
		{
			if (effectType == "draw")
			{
				_strategyContext = new StrategyContext(new DrawExecution(this._context));
			}
			else if (effectType == "lpchangeAdd" || effectType == "lpchangeReduce") 
			{
				_strategyContext = new StrategyContext(new LPChangeExecution(this._context));
			}
			else if (effectType == "returnCardFromFieldToHand")
			{
				_strategyContext = new StrategyContext(new ReturnCardFromFieldExecution(this._context));
			}
			else if (effectType == "returnSTFromGraveToHand")
			{
				_strategyContext = new StrategyContext(new ReturnSTCardFromGraveExecution(this._context));
			}
			else if (effectType == "returnMonsterFromGraveToHand" || effectType == "returnMonsterFromGraveToField")
			{
				_strategyContext = new StrategyContext(new ReturnMonsterCardFromGraveExecution(this._context));
			}
			else if (effectType == "takeCardFromEnemiesHand")
			{
				_strategyContext = new StrategyContext(new TakeCardFromEnemiesHandExecution(this._context));
			}
			else if (effectType == "takeCardFromEnemiesField")
			{
				_strategyContext = new StrategyContext(new TakeCardFromEnemiesFieldExecution(this._context));
			}
			else
			{
				throw new ArgumentException("Wrong type "+effectType);
			}
			return _strategyContext;
		}
	}
}
