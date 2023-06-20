using BLL.AttackingStrategy.Contracts;
using BLL.Services.Contracts;
using BLL.Services;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable
namespace BLL.AttackingStrategy
{
	public class AttackPositionStrategy : IAttackingStrategy
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }
		public AttackPositionStrategy(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			_playerService = new PlayerService(this._context);
			_gameService = new GameService(this._context);
		}

		public async Task<int> Attack(int gameID, CardField attackingField, CardField attackedField, MonsterCard attackingCard, MonsterCard attackedCard)
		{
			var pointLost = attackingCard.AttackPoints - attackedCard.AttackPoints;
			if(pointLost == 0)
			{
				await this._gameService.RemoveCardFromFieldToGrave(attackingField.ID, gameID, attackingField.PlayerID);
				await this._gameService.RemoveCardFromFieldToGrave(attackedField.ID, gameID, attackedField.PlayerID);
			}
			if (pointLost > 0)
			{
				await this._gameService.RemoveCardFromFieldToGrave(attackedField.ID, gameID, attackedField.PlayerID);
				var player = await this._unitOfWork.Player.GetOne(attackedField.PlayerID);
				player.LifePoints = player.LifePoints - pointLost;
				this._unitOfWork.Player.Update(player);
			}
			else if (pointLost < 0)
			{
				await this._gameService.RemoveCardFromFieldToGrave(attackingField.ID, gameID, attackingField.PlayerID);
				var player = await this._unitOfWork.Player.GetOne(attackingField.PlayerID);
				player.LifePoints = player.LifePoints + pointLost;
				this._unitOfWork.Player.Update(player);
			}
			return pointLost;
		}

		public Task<int> DirectAttack(CardField attackingField, MonsterCard attackingCard, int eneimiesID)
		{
			throw new NotImplementedException();
		}
	}
}
