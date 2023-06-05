using BLL.AttackingStrategy.Contracts;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AttackingStrategy
{
	public class AttackingStrategyContext
	{
		private readonly KnightsAndDiamondsContext _context;

		private IAttackingStrategy _attackingStrategy;
		public IUnitOfWork _unitOfWork { get; set; }
		//Constructor: assigns strategy to interface  
		public AttackingStrategyContext(KnightsAndDiamondsContext context,IAttackingStrategy strategy)
		{
			this._context = context;
			this._attackingStrategy = strategy;
			this._unitOfWork = new UnitOfWork(_context);

		}
		public async Task<int> AttackEnemiesField(CardField attackingField,CardField attackedField,int gameID)
		{
			var attackingCard = await this._unitOfWork.Card.GetMonsterCard(attackingField.CardOnField.CardID);
			var attackedCard = await this._unitOfWork.Card.GetMonsterCard(attackedField.CardOnField.CardID);
			return await this._attackingStrategy.Attack(gameID,attackingField,attackedField,attackingCard, attackedCard);
		}
	}
}
