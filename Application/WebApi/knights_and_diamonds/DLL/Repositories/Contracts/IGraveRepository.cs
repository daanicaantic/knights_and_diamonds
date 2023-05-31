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
		Task<Grave> GetGrave(int GraveID);
		Task<Grave> GetGraveByGameID(int gameID);
	}
}
