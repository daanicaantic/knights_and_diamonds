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

        public async Task<Player> GetPlayer(int rpsGameID, int userID)
        {
			var player = await this.Context.Players.Where(g => g.RPSGameID == rpsGameID && g.UserID == userID).FirstOrDefaultAsync();
			return player;
        }

        public async Task<Player> GetPlayerByID(int playerID)
        {
			var player = await this.Context.Players.Include(x => x.User).Where(p => p.ID == playerID).FirstOrDefaultAsync();
            return player;
        }

		public async Task<List<CardInDeck>> GetShuffledDeck(int playerID)
		{
			var deck = await this.Context.Players.Include(x => x.Deck).Where(p => p.ID == playerID).Select(x => x.Deck).FirstOrDefaultAsync();
			return deck;
        }
    }
}
