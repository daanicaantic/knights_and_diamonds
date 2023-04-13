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
	public class CardFieldRepository:Repository<CardField>,ICardFieldRepository
	{
		public CardFieldRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<CardField> GetCardField(int fieldID)
		{
			return await this.Context.CardFields.Where(x => x.ID == fieldID).Include(x => x.CardOnField).FirstOrDefaultAsync();
		}
	}
}
