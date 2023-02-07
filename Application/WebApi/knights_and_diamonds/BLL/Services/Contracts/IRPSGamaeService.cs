using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public interface IRPSGamaeService
	{
		void NewLobby(OnlineUserDto user, OnlineUserDto challengedUser);
		Task StartGame(int gameID);
		Task<Dictionary<int, List<int>>> GetGames();
		Task<Dictionary<int, List<OnlineUserDto>>> LobbiesPerUser(OnlineUserDto user);

	}
}
