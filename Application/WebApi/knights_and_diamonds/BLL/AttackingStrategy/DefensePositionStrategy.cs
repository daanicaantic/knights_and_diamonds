using BLL.AttackingStrategy.Contracts;
using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.AttackingStrategy
{
	public class DefensePositionStrategy : IAttackingStrategy
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }
		public DefensePositionStrategy(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			_playerService = new PlayerService(this._context);
			_gameService = new GameService(this._context);
		}

		public async Task<int> Attack(int gameID,CardField attackingField, CardField attackedField, MonsterCard attackingCard, MonsterCard attackedCard)
		{
			var pointLost = attackingCard.AttackPoints - attackedCard.DefencePoints;
			attackedField.CardShowen = true;
			if (pointLost > 0)
			{
				await this._gameService.RemoveCardFromFieldToGrave(attackedField.ID, gameID, attackedField.PlayerID);
				pointLost = 0;
			}
			else if (pointLost < 0)
			{
				var player=await this._unitOfWork.Player.GetOne(attackingField.PlayerID);
				player.LifePoints=player.LifePoints+pointLost;
				this._unitOfWork.Player.Update(player);
			}
			this._unitOfWork.CardField.Update(attackedField);
			return pointLost;
			
		}

		public Task<int> DirectAttack(CardField attackingField, MonsterCard attackingCard, int eneimiesID)
		{
			throw new NotImplementedException();
		}
	}
}
