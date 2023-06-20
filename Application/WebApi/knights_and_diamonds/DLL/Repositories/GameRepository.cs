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
			var game = await this.Context.Games?
				.Include(x => x.Players)
				.Where(x => x.ID == gameID)
				.FirstOrDefaultAsync();
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
			return game;
		}

		public async Task<Game> GetGameWithTurns(int gameID)
        {
            var game = await this.Context.Games?
                .Include(x => x.Turns)
                .Where(x => x.ID == gameID)
                .FirstOrDefaultAsync();
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
			return game;
        }
		public async Task<int> GetEnemiesPlayerID(int gameID,int playerID)
		{
			var enemiesID = await this.Context.Players?
				.Where(x => x.GameID == gameID && x.ID!=playerID)
				.Select(x=>x.ID)
				.FirstOrDefaultAsync();
			return enemiesID;
		}

	}
}
