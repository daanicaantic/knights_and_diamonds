using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class ConnectionService : IConnectionService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		public ConnectionService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
		}
		public async Task<Connection> GetConnection(int id)
		{
			try
			{
				return await this.unitOfWork.Connection.GetOne(id);
			}
			catch
			{
				throw;
			}
		}
		public async Task<Connection> GetConnectionByUser(int Userid)
		{
			try
			{
				return await this.unitOfWork.Connection.GetConnectionByUser(Userid);
			}
			catch
			{
				throw;
			}
		}

		public async Task<IEnumerable<Connection>> GetAllConnections()
		{
			try 
			{
				return await this.unitOfWork.Connection.GetAll();
			}
			catch 
			{
				throw;
			}
		}

		public void RemoveConnection(Connection connection)
		{
			try
			{
				this.unitOfWork.Connection.Delete(connection);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}
		public void AddConnection(Connection connection)
		{
			try
			{
				this.unitOfWork.Connection.Add(connection);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}

		

		public void UpdateConnection(Connection connection)
		{
			this.unitOfWork.Connection.Update(connection);
			this.unitOfWork.Complete();
		}

	}
}
