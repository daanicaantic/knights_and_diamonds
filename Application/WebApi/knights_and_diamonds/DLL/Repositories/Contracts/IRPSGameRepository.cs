using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface IRPSGameRepository : IRepository<RockPaperScissorsGame>
	{
        Task<RockPaperScissorsGame> GetGameWithPlayers(int gameID);
		Task<int> GetRpsGameWinner(int rpsGameID);

	}
}
