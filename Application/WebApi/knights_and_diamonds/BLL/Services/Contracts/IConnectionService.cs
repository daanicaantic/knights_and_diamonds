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
		List<string> GetConnectionByUser(int Userid);
		Task<List<OnlineUserDto>> GetOnlineUsers();
		void AddOnlineUser(int userID, string connectionID);
		void RemoveUserFromOnlineUsers(int userID);
	}
}
