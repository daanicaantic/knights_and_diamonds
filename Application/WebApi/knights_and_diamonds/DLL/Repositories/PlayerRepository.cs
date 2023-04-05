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

		public void AddPlayer(Player p)
		{

		}

        public async Task<Player> GetPlayer(int rpsGameID, int userID)
        {
			var player = await this.Context.Players.Where(g => g.RPSGameID == rpsGameID && g.UserID == userID).FirstOrDefaultAsync();
			return player;
        }

        public async Task<Player> GetPlayerByID(int playerID)
        {
			var player = await this.Context.Players
				.Include(x => x.User)
				.Include(x => x.Deck)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
            return player;
        }

		public async Task<Player> GetPlayerWithHandAndDeckByID(int playerID)
		{
			var player = await this.Context.Players
				.Include(x=>x.Deck)
				.ThenInclude(x=>x.Card)
				.Include(x => x.Hand)
				.ThenInclude(x => x.CardsInHand)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			return player;
		}

		public async Task<PlayersHand> GetPlayersHand(int playerID)
		{
			var playerHand = await this.Context.Players
				.Include(x => x.Hand)
				.ThenInclude(x => x.CardsInHand)
				.ThenInclude(x => x.Card)
				.Where(p => p.ID == playerID)
				.Select(x=>x.Hand)
				.FirstOrDefaultAsync();
			return playerHand;
		}
    }
}
