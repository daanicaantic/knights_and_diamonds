using DAL.DesignPatterns;
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
        Task<int> NewLobby(OnlineUserDto user, OnlineUserDto challengedUser);
        Task<int> StartGame(int lobbyID);
        Task<int> DenyGame(int lobbyID);
        Task<Dictionary<int, List<int>>> GetGames();
		Task<List<Lobby>> LobbiesPerUser(int userID);
        Task<List<int>> RedirectToGame(int gameID);

    }
}
