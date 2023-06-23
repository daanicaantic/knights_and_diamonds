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
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BLL.AttackingStrategy
{
	public class DirectAttackStrategy : IAttackingStrategy
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }
		public DirectAttackStrategy(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(this._context);
			this._gameService = new GameService(this._context);
		}
		public Task<int> Attack(int gameID, CardField attackingField, CardField attackedField, MonsterCard attackingCard, MonsterCard? attackedCard)
		{
			throw new NotImplementedException();
		}

		public async Task<int> DirectAttack(CardField attackingField, MonsterCard attackingCard,int eneimiesID)
		{
			var fields = await this._unitOfWork.CardField.GetPlayerFields(eneimiesID, "MonsterField");
			foreach (var field in fields)
			{
				if (field.CardOnField != null)
				{
					throw new Exception("You cannot attack directly");
				}
			}
			var player = await this._unitOfWork.Player.GetOne(eneimiesID);
			player.LifePoints = player.LifePoints - attackingCard.AttackPoints;
			this._unitOfWork.Player.Update(player);
			return attackingCard.AttackPoints;
		}
	}
}
