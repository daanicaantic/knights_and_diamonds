using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class AttackInTurnsRepository : Repository<AttackInTurn>, IAttackInTurnRepository
	{
		public AttackInTurnsRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext? Context
		{
			get
			{
				return _context as KnightsAndDiamondsContext; 
			}
		}
		public async Task<AttackInTurn> GetAttackInTurnByTurnIDAndFieldID(int fieldID, int turnID)
		{
			var posibleAttack = await this.Context.AttackInTurns?.Where(x => x.CardFieldID == fieldID && x.TurnID == turnID).FirstOrDefaultAsync();
			if (posibleAttack == null)
			{
				throw new Exception("This field cannot attack in this turn,there is some errors");
			}
			if (posibleAttack.IsAttackingAbble == false)
			{
				throw new Exception("You allready attack in this turn from this field");
			}
			return posibleAttack;

		}
	}
}
