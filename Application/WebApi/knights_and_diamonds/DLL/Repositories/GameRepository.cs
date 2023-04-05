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
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(KnightsAndDiamondsContext context) : base(context)
        {

        }
        public KnightsAndDiamondsContext Context
        {
            get { return _context as KnightsAndDiamondsContext; }
        }
		public async Task<Game> GetGameWithPlayers(int gameID)
		{
			var game = await this.Context.Games
				.Include(x => x.Players)
				.Where(x => x.ID == gameID)
				.FirstOrDefaultAsync();
			return game;
		}

		public async Task<Game> GetGameWithTurns(int gameID)
        {
            var game = await this.Context.Games
                .Include(x => x.Turns)
                .Where(x=>x.ID==gameID)
                .FirstOrDefaultAsync();
            return game;
        }
    }
}
