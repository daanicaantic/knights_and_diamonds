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
	public class RPSGameRepository: Repository<RockPaperScissorsGame>,IRPSGameRepository
	{
		public RPSGameRepository(KnightsAndDiamondsContext context) : base(context)
		{

		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<IEnumerable<RockPaperScissorsGame>> GetGamesWithPlayers()
		{
			try
			{
				return await this.Context.RockPaperScissorsGames
					.Include(x=>x.Players)
					.ToListAsync();
			}
			catch
			{
				throw;
			}
		}
	}
}
