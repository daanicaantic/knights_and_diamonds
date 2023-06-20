using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DTOs;
using DAL.Models;
using DAL.Repositories.Contracts;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
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
		public UnitOfWork _unitOfWork { get; set; }
		public OnlineUsers _onlineUsers { get; set; }
		public InGameUsers _inGameUsers { get; set; }

        public ConnectionService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_unitOfWork = new UnitOfWork(_context);
			_onlineUsers = OnlineUsers.GetInstance();
			_inGameUsers = InGameUsers.GetInstance();
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
			else if (!_onlineUsers.ConnectedUsers[userID].Contains(connectionId))
            {
				_onlineUsers.ConnectedUsers[userID].Add(connectionId);
			}
		}
		public List<string> GetConnectionByUser(int UserID)
		{
			var cons = new List<string>();
			if (_onlineUsers.ConnectedUsers.ContainsKey(UserID)) 
			{ 
				cons =_onlineUsers.ConnectedUsers[UserID].ToList();
				if (cons == null)
				{
					throw new Exception("This user has no connections");
				}
			}
			return cons;
		}
		public async Task<List<OnlineUserDto>> GetOnlineUsers()
		{
			#pragma warning disable
			OnlineUserDto onlineUserDto;
			List<OnlineUserDto> ListOfOnlineUsers = new List<OnlineUserDto>();
			if (this._onlineUsers.ConnectedUsers.Keys.Count == 0)
			{
				throw new Exception("There is no online users");
			}
			foreach (var userID in this._onlineUsers.ConnectedUsers.Keys)
			{
                var user = await this._unitOfWork.User.GetOne(userID);
                if (user != null)
                {
                    onlineUserDto = new OnlineUserDto(user.ID, user.Name, user.SurName, user.UserName);
                    ListOfOnlineUsers.Add(onlineUserDto);
                }
                else
                {
                    throw new Exception("This user does not exsists.");
                }

            }
            return ListOfOnlineUsers;
		}
		public void RemoveUserFromOnlineUsers(int userID)
		{
			if (_onlineUsers.ConnectedUsers.ContainsKey(userID))
			{
				this._onlineUsers.ConnectedUsers.Remove(userID);
				var lobbies = this._inGameUsers.Lobbies.Where(x => x.User1.ID == userID).ToList();
				foreach (var lobby in lobbies)
				{
					this._inGameUsers.Lobbies.Remove(lobby);
				}
			}
		}
	}
}
