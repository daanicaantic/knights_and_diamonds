using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class EffectService : IEffectService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		public IEffectFactory _descriptionFactory { get; set; }
		public IFactory _factory { get; set; }
		public EffectService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			this._descriptionFactory = new ConcreteEffectFactory();
		}

		public async Task<IList<EffectType>> GetEffectTypes()
		{
			return await this.unitOfWork.Effect.GetEffectTypes();
		}

		public async Task<EffectType> GetEffectTypeByID(int effectTypeID)
		{
			return await this.unitOfWork.Effect.GetEffectType(effectTypeID);
		}

/*		public async Task<Effect> AddEffect(int effectTypeID,int numOfCardsAffected, int pointsAddLost)
		{
			Effect effect;
			var effectType = await this.unitOfWork.Effect.GetEffectType(effectTypeID);
			if (effectType == null) { throw new Exception("There is no effectType with this ID."); }

			var type = await this.SplitType(effectType.Type);
			var description = await this.GetDescription(type, effectType.Type, numOfCardsAffected, pointsAddLost);

			effect = await this.unitOfWork.Effect.GetEffectByDescription(description);
			if (effect == null)
			{
				effect = new Effect(effectTypeID, effectType, description, numOfCardsAffected, pointsAddLost);
				await this.unitOfWork.Effect.AddEffect(effect);
				return effect;

			}

			return effect;
		}*/

		
	}
}
