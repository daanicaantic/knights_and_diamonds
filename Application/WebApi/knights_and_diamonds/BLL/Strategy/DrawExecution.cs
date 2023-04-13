using BLL.Services;
using BLL.Services.Contracts;
using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class DrawExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public DrawExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			_playerService = new PlayerService(this._context);
		}
		public ChooseCardsFrom SelectCardsFrom()
		{
			return ChooseCardsFrom.NoChoose;
		}
		public async Task ExecuteEffect(List<int> listOfCards,string description,int playerID)
		{
			var effect = await this.GetEffect(description);
			while (effect.PointsAddedLost != 0)
			{
				effect.PointsAddedLost--;
				await this._playerService.Draw(playerID);
			}
		}
		public async Task<Effect> GetEffect(string description)
		{
			var effect = await this._unitOfWork.Effect.GetEffectByDescription(description);
			if (effect == null)
			{
				throw new Exception("There is no effect with this description");
			}
			if (effect.EffectType.Type != "draw")
			{
				throw new Exception("There is some Error");
			}
			return effect;
		}
	}
}
