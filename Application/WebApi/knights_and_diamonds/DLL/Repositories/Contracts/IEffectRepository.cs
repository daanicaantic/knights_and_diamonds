﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface IEffectRepository : IRepository<Effect>
	{
		Task<Effect> AddEffect(Effect effect);
		Task<IList<EffectType>> GetEffectTypes();
		Task<EffectType> GetEffectType(int EffectTypeID);
		Task<Effect> GetEffectByDescription(string description);
	}
}
