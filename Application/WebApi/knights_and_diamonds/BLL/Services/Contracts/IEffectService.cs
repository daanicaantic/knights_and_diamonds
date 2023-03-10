using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public interface IEffectService
	{
		Task<IList<EffectType>> GetEffectTypes();
		Task<Effect> AddEffect(int effectTypeID, int numOfCardsAffected, int pointsAddLost);
		Task<string> SplitType(string type);
	}
}
