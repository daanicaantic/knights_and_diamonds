using BLL.Services.Contracts;
using BLL.Strategy;
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
		public UnitOfWork _unitOfWork { get; set; }
		public IEffectFactory _descriptionFactory { get; set; }
		public IFactory _factory { get; set; }
		public StrategyContext strategyContext { get; set; }
		public ConcreteStrategy concreteStrategy { get; set; }

		public EffectService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_unitOfWork = new UnitOfWork(_context);
			this._descriptionFactory = new ConcreteEffectFactory();
			this.concreteStrategy = new ConcreteStrategy(this._context);
		}

		public async Task<IList<EffectType>> GetEffectTypes()
		{
			return await this._unitOfWork.Effect.GetEffectTypes();
		}

		public async Task<EffectType> GetEffectTypeByID(int effectTypeID)
		{
			return await this._unitOfWork.Effect.GetEffectType(effectTypeID);
		}

		public async Task<int> GetAreaOfClickingAfterPlayCard(int effectTypeID)
		{
			var effectType = await this._unitOfWork.Effect.GetEffectType(effectTypeID);
			if (effectType == null)
			{
				throw new Exception("There is no effectType with this ID");
			}
			this.strategyContext = concreteStrategy.SetStrategyContext(effectType.Type);
			var area = strategyContext.GetAreaOfSelectingCards();
			return area;
		}
	}
}
