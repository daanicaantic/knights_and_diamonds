using BLL.Services.Contracts;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class ReturnMonsterCardFromGraveExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;

		public ReturnMonsterCardFromGraveExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}
		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.GraveMonsteCard;
		}
		public Task ExecuteEffect(List<int> listOfCards, string description, int playerID)
		{
			throw new NotImplementedException();
		}
	}
}
