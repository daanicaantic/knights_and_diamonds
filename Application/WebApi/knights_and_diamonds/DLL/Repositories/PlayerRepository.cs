using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class PlayerRepository : Repository<Player>, IPlayerRepository
	{
		public PlayerRepository(KnightsAndDiamondsContext context) : base(context)
		{

		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

        public async Task<Player> GetPlayer(int gameID, int userID)
        {
			var player = await this.Context.Players.Where(g => g.RPSGameID == gameID && g.UserID == userID).FirstOrDefaultAsync();
			return player;
        }
    }
}
