using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.DataContext;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly DbContext _context;
		public Repository(DbContext context)
		{
			this._context = context;
		}
		public virtual T GetOne(int id)
		{
			return this._context.Set<T>().Find(id);
		}
		public IEnumerable<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}
		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return _context.Set<T>().Where(predicate);
		}
		public void Add(T obj)
		{
			this._context.Set<T>().Add(obj);
		}
		public void Delete(T obj)
		{

			this._context.Set<T>().Remove(obj);
		}
		public void Update(T obj)
		{
			this._context.Set<T>().Update(obj);
		}
	}
}
