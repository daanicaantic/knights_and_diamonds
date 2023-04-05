using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IGameService
    {
		Task<GameDTO> GetGame(int gameID, int userID);
		Task<int> SetFirstGamesTurn(int rpsGameID, int gameID);
		Task<List<string>> GameGroup(int gameID);
		Task<ConnectionsPerUser> GameConnectionsPerPlayer(int gameID, int playerID);

	}
}
