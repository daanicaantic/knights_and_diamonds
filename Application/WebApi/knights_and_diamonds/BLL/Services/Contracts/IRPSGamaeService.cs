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
		void NewLobby(User user,User challengedUser);
		Task StartGame(int gameID);
		Task<Dictionary<int, List<int>>> GetGames();
		Task<Dictionary<int, List<int>>> LobbiesPerUser(int userID);

	}
}
