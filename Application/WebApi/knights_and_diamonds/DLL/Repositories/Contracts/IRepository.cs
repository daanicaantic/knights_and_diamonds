using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Text;
using System.Threading.Tasks;

using DAL.Models;
namespace DAL.Repositories.Contracts
{
	public interface IRepository<T> where T : class
	{
		Task<T> GetOne(int id);
		Task<IEnumerable<T>> GetAll();
		IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
		void Add(T obj);
		void Delete(T obj);
		void Update(T obj);
	}
}
