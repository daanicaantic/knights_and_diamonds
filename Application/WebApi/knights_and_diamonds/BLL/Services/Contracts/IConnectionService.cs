using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public interface IConnectionService
	{
		Task<Connection> GetConnection(int id);
		Task<Connection> GetConnectionByUser(int Userid);
		Task<IEnumerable<Connection>> GetAllConnections();
		Task<List<OnlineUserDto>> GetOnlineUsers();
		void AddConnection(Connection connection);
		void AddUserIDtoList(int userID);
		void RemoveConnection(Connection connection);
		void RemoveUserFromOnlineUsers(int userID);
		void UpdateConnection(Connection connection);
	}
}
