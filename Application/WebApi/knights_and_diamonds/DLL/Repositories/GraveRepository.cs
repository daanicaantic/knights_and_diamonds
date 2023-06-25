using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DAL.Repositories.Contracts.ICardRepository;
using static DAL.Repositories.Contracts.IGraveRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repositories
{
	public class GraveRepository : Repository<Grave>, IGraveRepository
	{
		public GraveRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext? Context
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
			var grave = await this.Context.Games?.Include(x => x.Grave)?
                .ThenInclude(x => x.ListOfCardsInGrave)
                .ThenInclude(x => x.Card)
                .Where(x => x.ID == gameID)
                .Select(x => x.Grave)
                .FirstOrDefaultAsync();
			if (grave == null)
			{
				throw new Exception("There is no grave with this gameid");
			}
			return grave;
		}
        public async Task<FilteredGrave> GetGraveByType(int gameID,string type,int pageSize,int pageNumber)
		{
			var fitleredGrave = new FilteredGrave();
			var graveID = await this.Context?.Games?.Where(x => x.ID == gameID).Select(x => x.GraveID).FirstOrDefaultAsync();
			var query = this.Context?.CardInDecks?.Include(x => x.Grave)
			.Include(x => x.Card)
			.Where(x => x.Grave.ID == graveID && (string.IsNullOrEmpty(type) || x.Card.Discriminator == type));

            int totalItems = await query?.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
			query = query?.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var cards = await query?.ToListAsync();
			fitleredGrave.Cards = cards;
			fitleredGrave.PageNumber = pageNumber;
			fitleredGrave.TotalPages = totalPages;
			fitleredGrave.TotalItems = totalItems;
			fitleredGrave.PageSize = pageSize;

			return fitleredGrave;
        }


    }
}
