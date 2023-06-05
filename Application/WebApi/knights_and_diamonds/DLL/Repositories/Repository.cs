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
		public async Task<T> GetOne(int id)
		{
			var obj=await this._context.Set<T>().FindAsync(id);
			if (obj == null)
			{
				throw new Exception("Object with this ID dosent exists");
			}
			return obj;
		}
		public async Task<IQueryable<T>> GetAll()
		{
			return (IQueryable<T>)await _context.Set<T>().ToListAsync();
		}
		public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return _context.Set<T>().Where(predicate);
		}
		public async Task Add(T obj)
		{
			await this._context.Set<T>().AddAsync(obj);
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
