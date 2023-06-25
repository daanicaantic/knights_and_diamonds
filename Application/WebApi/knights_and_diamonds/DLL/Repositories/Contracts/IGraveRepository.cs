using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface IGraveRepository:IRepository<Grave>
	{
        public struct FilteredGrave
        {
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public List<CardInDeck>? Cards { get; set; }
        }
        Task<Grave> GetGrave(int GraveID);
		Task<Grave> GetGraveByGameID(int gameID);
        Task<FilteredGrave> GetGraveByType(int gameID, string type, int pageSize, int pageNumber);

    }
}
