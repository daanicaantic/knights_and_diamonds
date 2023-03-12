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
	public class EffectRepository:Repository<Effect>,IEffectRepository
	{
		public EffectRepository(KnightsAndDiamondsContext context) : base(context)
		{

		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<Effect> AddEffect(Effect effect) 
		{
			this.Context.Effects.Include(x => x.EffectType);
			this.Context.Effects.Add(effect);
			return effect;
		}
		public async Task<Effect> GetEffectByDescription(string description)
		{
			return await this.Context.Effects.Where(x=>x.Description==description).FirstOrDefaultAsync();
		}
		public async Task<EffectType> GetEffectType(int EffectTypeID)
		{
			return await this.Context.EffectTypes.FindAsync(EffectTypeID);
		}

		public async Task<IList<EffectType>> GetEffectTypes()
		{
			return await this.Context.EffectTypes.ToListAsync();
		}
	}
}
