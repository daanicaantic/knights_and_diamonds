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
		enum GamePhase
		{
			DrawPhase,
			MainPhase,
			BeatlePhase,
			EndPhase,
		}
		Task<Game> GetGameByID(int gameID);
		Task<GameDTO> GetGame(int gameID, int userID);
		Task<List<string>> GameGroup(int gameID);
		Task<ConnectionsPerUser> GameConnectionsPerPlayer(int gameID, int playerID);
		Task<FieldDTO> GetPlayersField(int playerID);
		EnemiesFieldDTO GetEneiesField(FieldDTO enemiesField);
		Task<List<MappedCard>> DrawPhase(int gameID);
		Task<Game> NewTurn(int gameID);
		Task<Turn> GetTurn(int gameID);
		Task<GamePhase> GetGamePhase(int gameID);
		Task<int> GetPlayerOnTurn(int gameID);

	}
}
