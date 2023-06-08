using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
    public interface IGameRepository : IRepository<Game>
    {
		Task<Game> GetGameWithPlayers(int gameID);
		Task<Game> GetGameWithTurns(int gameID);
		Task<int> GetEnemiesPlayerID(int gameID, int playerID);

	}
}
