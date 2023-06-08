using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AttackingStrategy.Contracts
{
	public interface IAttackingStrategy
	{
		Task<int> Attack(int gameID, CardField attackingField, CardField attackedField, MonsterCard attackingCard, MonsterCard? attackedCard);
		Task<int> DirectAttack(CardField attackingField,MonsterCard attackingCard, int eneimiesID);

	}
}
