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
	public class TakeCardFromEnemiesHandExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public IPlayerService _playerService { get; set; }
		public IGameService _gameService { get; set; }

		public TakeCardFromEnemiesHandExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
			this._unitOfWork = new UnitOfWork(_context);
			this._playerService = new PlayerService(_context);
			this._gameService = new GameService(_context);
		}

		public ChooseCardsFrom SelectCardsFrom()
		{
			Console.WriteLine("Enemies hand");
			return ChooseCardsFrom.EnemiesHand;
		}

		public async Task ExecuteEffect(List<int> listOfCardIDs, Effect effect, int playerID, int gameID, int fieldID)
		{
            if (effect.NumOfCardsAffected != listOfCardIDs.Count)
            {
                throw new Exception("You didn't select enough cards.");
            }

            var enemieID = await this._unitOfWork.Game.GetEnemiesPlayerID(gameID, playerID);

            var enemiesHand = await this._unitOfWork.Player.GetPlayersHand(enemieID);

            if (enemiesHand == null)
            {
                throw new Exception("This player doesn't have hands :(");
            }

            if (enemiesHand.CardsInHand.Count < effect.NumOfCardsAffected)
            {
                throw new Exception("This player doesn't have enough cards in his hand.");
            }
            
			foreach (var cardID in listOfCardIDs)
			{
				await _playerService.TakeCardFromEnemiesHand(enemiesHand, playerID, cardID);
            }

			await this._gameService.RemoveCardFromFieldToGrave(fieldID, gameID, playerID);
        }
	}
}
