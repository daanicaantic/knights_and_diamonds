using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
			var player = await this.Context.Players?.Where(g => g.RPSGameID == rpsGameID && g.UserID == userID).FirstOrDefaultAsync();
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return player;
        }

        public async Task<Player> GetPlayerByID(int playerID)
        {
			var player = await this.Context.Players?
				.Include(x => x.User)
				.Include(x => x.Deck)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return player;
        }

		public async Task<Player> GetPlayerWithHandAndDeckByID(int playerID)
		{
			#pragma warning disable
			var player = await this.Context.Players?
				.Include(x=>x.Deck)
				.ThenInclude(x=>x.Card)
				.Include(x => x.Hand)
				.ThenInclude(x => x.CardsInHand)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return player;
		}

		public async Task<PlayersHand> GetPlayersHand(int playerID)
		{
			var playerHand = await this.Context.Players?
				.Include(x => x.Hand)
				.ThenInclude(x => x.CardsInHand)
				.ThenInclude(x => x.Card)
				.Where(p => p.ID == playerID)
				.Select(x=>x.Hand)
				.FirstOrDefaultAsync();
			if (playerHand == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return playerHand;
		}

		public async Task<Player> GetPlayersField(int playerID)
		{
			#pragma warning disable
			var field = await this.Context.Players?
				.Include(x => x.Hand)?
				.ThenInclude(x => x.CardsInHand)?
				.ThenInclude(x => x.Card)?
				.Include(x => x.Deck)?
				.ThenInclude(x => x.Card)
				.Include(x => x.Fields)
				.ThenInclude(x => x.CardOnField)
				.ThenInclude(x => x.Card)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			if (field == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return field;
		}

		public async Task<Player> GetPlayerWithFieldsAndHand(int playerID)
		{
			var player = await this.Context.Players?
				.Include(x => x.Fields)
				.ThenInclude(x => x.CardOnField)
				.Include(x=>x.Hand)
				.ThenInclude(x=>x.CardsInHand)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return player;
		}
		public async Task<Player> GetPlayersFields(int playerID)
		{
			var player = await this.Context.Players?
				.Include(x => x.Fields)
				.ThenInclude(x => x.CardOnField)
				.Where(p => p.ID == playerID)
				.FirstOrDefaultAsync();
			if (player == null)
			{
				throw new Exception("There is no player with this ID");
			}
			return player;
		}
		public async Task UpdateHandByPlayerID(int playerID)
		{
			var hand = await this.Context.Players?.Include(x => x.Hand).ThenInclude(x => x.CardsInHand).Where(x => x.ID == playerID).Select(x => x.Hand).FirstOrDefaultAsync();
			if (hand == null)
			{
				throw new Exception("There is no hand with this playerID");
			}
			this.Context.PlayerHands.Update(hand);
		}
	}
}
