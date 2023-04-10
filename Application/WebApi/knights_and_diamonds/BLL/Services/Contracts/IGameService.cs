using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IGameService
    {
		Task<Game> GetGameByID(int gameID);
		Task<GameDTO> GetGame(int gameID, int userID);
		Task<int> SetFirstGamesTurn(int rpsGameID, int gameID);
		Task<List<string>> GameGroup(int gameID);
		Task<ConnectionsPerUser> GameConnectionsPerPlayer(int gameID, int playerID);
		Task<FieldDTO> GetPlayersField(int playerID);
		EnemiesFieldDTO GetEneiesField(FieldDTO enemiesField);

	}
}
