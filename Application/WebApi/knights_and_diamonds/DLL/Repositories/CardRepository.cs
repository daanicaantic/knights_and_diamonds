using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class CardRepository : Repository<Card>,ICardRepository
	{
		
		public CardRepository(KnightsAndDiamondsContext context) : base(context)
		{
			
		}

		public IEnumerable<Card> GetCardsPerPage(int pageIndex, int pageSize)
		{
			return Context.Cards
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToList();
		}
		public KnightsAndDiamondsContext Context
		{
			get { return Context as KnightsAndDiamondsContext; }
		}
	}
}
