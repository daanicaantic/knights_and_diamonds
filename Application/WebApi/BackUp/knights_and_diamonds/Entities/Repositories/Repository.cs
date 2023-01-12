using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.DataContext;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly KnightsAndDiamondsContext _context;
		public Repository(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}
		public bool Add(T obj)
		{
			this._context.Set<T>().Add(obj);
			this._context.SaveChanges();
			return true;
		}
	}
}
