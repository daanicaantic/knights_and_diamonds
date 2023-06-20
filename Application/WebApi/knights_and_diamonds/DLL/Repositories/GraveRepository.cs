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
	public class GraveRepository : Repository<Grave>, IGraveRepository
	{
		public GraveRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<Grave> GetGrave(int GraveID)
		{
			var grave = await this.Context.Graves?.Include(x => x.ListOfCardsInGrave).Where(x => x.ID == GraveID).FirstOrDefaultAsync();
			if (grave == null)
			{
				throw new Exception("There is no grave with this id");
			}
			return grave;
		}
		public async Task<Grave> GetGraveByGameID(int gameID)
		{
			var grave = await this.Context.Games?.Include(x => x.Grave)?.ThenInclude(x => x.ListOfCardsInGrave).ThenInclude(x => x.Card).Where(x => x.ID == gameID).Select(x => x.Grave).FirstOrDefaultAsync();
			if (grave == null)
			{
				throw new Exception("There is no grave with this gameid");
			}
			return grave;
		}
	}
}
