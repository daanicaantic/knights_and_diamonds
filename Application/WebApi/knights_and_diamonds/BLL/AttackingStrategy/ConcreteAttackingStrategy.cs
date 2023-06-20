using BLL.Strategy;
using DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AttackingStrategy
{
	public class ConcreteAttackingStrategy
	{
		private readonly KnightsAndDiamondsContext _context;
		public	AttackingStrategyContext? _strategyContext { get; set; }
		public ConcreteAttackingStrategy(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}
		public AttackingStrategyContext SetStrategyContext(bool cardPosition,string fieldType)
		{
			if (fieldType == "SpellTrapField")
			{
				return _strategyContext = new AttackingStrategyContext(this._context,new DirectAttackStrategy(this._context));
			}
			else if (cardPosition==true && fieldType=="MonsterField")
			{
				return _strategyContext = new AttackingStrategyContext(this._context,new AttackPositionStrategy(this._context));
			}
			else
			{
				return _strategyContext = new AttackingStrategyContext(this._context, new DefensePositionStrategy(this._context));
			}
		}
	}
}
