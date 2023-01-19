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
	public class ConnectionRepository : Repository<Connection>, IConnectionRepository
	{
		public ConnectionRepository(DbContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<Connection> GetConnectionByUser(int UserId)
		{
			try
			{
				return await this.Context.Connections.Where(x => x.UserID == UserId).FirstOrDefaultAsync();
			}
			catch
			{
				throw;
			}
		}
	}
}
