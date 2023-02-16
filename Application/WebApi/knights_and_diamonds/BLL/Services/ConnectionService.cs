using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;
using DAL.Models;
using DAL.Repositories.Contracts;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Server.IIS.Core;
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

		public void AddOnlineUser(int userID, string connectionId)
		{
			List<string> connectionsIDs;
			if (_onlineUsers.ConnectedUsers.Count < 0)
			{
				throw new Exception("No one is logged in.");
			}
			if (!_onlineUsers.ConnectedUsers.ContainsKey(userID))
			{
				connectionsIDs = new List<string>();
				connectionsIDs.Add(connectionId);
				_onlineUsers.ConnectedUsers.Add(userID, connectionsIDs);
			}
			else
			{
				_onlineUsers.ConnectedUsers[userID].Add(connectionId);
			}
		}
		public async Task<List<string>> GetConnectionByUser(int UserID)
		{
			var cons = new List<string>();
			if (_onlineUsers.ConnectedUsers.ContainsKey(UserID)) 
			{ 
				cons = _onlineUsers.ConnectedUsers[UserID].ToList();
			}
			return cons;
		}
		public async Task<List<OnlineUserDto>> GetOnlineUsers()
		{
			OnlineUserDto onlineUserDto;
			List<OnlineUserDto> ListOfOnlineUsers = new List<OnlineUserDto>();
			foreach (var userID in this._onlineUsers.ConnectedUsers.Keys)
			{
				var user = await this.unitOfWork.User.GetOne(userID);
				if (user != null)
				{
					onlineUserDto = new OnlineUserDto(user.ID, user.Name, user.SurName, user.UserName);
					ListOfOnlineUsers.Add(onlineUserDto);
				}
				else 
				{
					throw new Exception("This" + user.ID.ToString() + "does not exsists.");
				}
			}
			return ListOfOnlineUsers;
		}
		public void RemoveUserFromOnlineUsers(int userID)
		{
			if (_onlineUsers.ConnectedUsers.ContainsKey(userID))
			{
				this._onlineUsers.ConnectedUsers.Remove(userID);
			}
		}
	}
}
