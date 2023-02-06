using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;
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
		public OnlineUsers _onlineUsers { get; set; }

		public ConnectionService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			_onlineUsers = OnlineUsers.GetInstance();
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

		public async Task<List<OnlineUserDto>> GetOnlineUsers()
		{
			try
			{
				OnlineUserDto userDTO;
				List<OnlineUserDto> ListOfOnlineUsers = new List<OnlineUserDto>();
				foreach (var userID in this._onlineUsers.ConnectedUsers)
				{
					var user=await this.unitOfWork.User.GetOne(userID);
					if (user != null) 
					{
						userDTO = new OnlineUserDto(user.ID, user.Name, user.SurName, user.UserName);
						ListOfOnlineUsers.Add(userDTO);
					}
				}
				return ListOfOnlineUsers;
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

		public void RemoveUserFromOnlineUsers(int userID)
		{
			if (_onlineUsers.ConnectedUsers.Contains(userID)) 
			{ 
				this._onlineUsers.ConnectedUsers.Remove(userID);
			}
		}
		public void AddConnection(Connection connection)
		{
			if (!_onlineUsers.ConnectedUsers.Contains(connection.UserID))
			{
				this.unitOfWork.Connection.Add(connection);
				this._onlineUsers.ConnectedUsers.Add(connection.UserID);
				this.unitOfWork.Complete();
			}
			else
			{
				throw new Exception("User is already logedin");
			}
		}
		/*koristimo funkciju ukoliko jos uvek nije istekla 
		 * prethodna konekcija korisnkia,*/
		public void AddUserIDtoList(int userID) 
		{
			if (!_onlineUsers.ConnectedUsers.Contains(userID)) 
			{ 
				this._onlineUsers.ConnectedUsers.Add(userID);
			}
		}

		public void UpdateConnection(Connection connection)
		{
			this.unitOfWork.Connection.Update(connection);
			this.unitOfWork.Complete();
		}

	}
}
