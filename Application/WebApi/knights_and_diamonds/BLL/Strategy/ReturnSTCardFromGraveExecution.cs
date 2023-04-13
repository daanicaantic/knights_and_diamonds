using BLL.Strategy.Context;
using DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class ReturnSTCardFromGraveExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;

		public ReturnSTCardFromGraveExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}
		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.GraveSTCard;
		}
		public Task ExecuteEffect(List<int> listOfCards, int effectID)
		{
			throw new NotImplementedException();
		}

		public Task ExecuteEffect(List<int> listOfCards, string description, int playerID)
		{
			throw new NotImplementedException();
		}
	}
}
