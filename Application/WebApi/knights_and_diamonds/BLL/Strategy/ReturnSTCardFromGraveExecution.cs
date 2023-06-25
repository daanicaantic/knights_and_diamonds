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
	public class ReturnSTCardFromGraveExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public IPlayerService _playerService { get; set; }
        public IGameService _gameService { get; set; }

        public ReturnSTCardFromGraveExecution(KnightsAndDiamondsContext context)
		{
            this._context = context;
            this._unitOfWork = new UnitOfWork(_context);
            this._playerService = new PlayerService(_context);
            this._gameService = new GameService(_context);
        }

		public ChooseCardsFrom SelectCardsFrom()
		{
			Console.WriteLine("Grave ST Card");
            return ChooseCardsFrom.GraveSTCard;
		}

		public async Task ExecuteEffect(List<int> listOfCardIDs, Effect effect, int playerID, int gameID, int fieldID)
		{
            if (effect.NumOfCardsAffected != listOfCardIDs.Count)
            {
                throw new Exception("You didn't select enough cards.");
            }

            var grave = await this._unitOfWork.Grave.GetGraveByGameID(gameID);
            if (grave == null)
            {
                throw new Exception("Grave doesn't exists.");
            }

            if(grave.ListOfCardsInGrave.Count < listOfCardIDs.Count)
            {
                throw new Exception("Grave doesn't have enough cards for card effect.");
            }

            foreach (var cardID in listOfCardIDs)
            {
          /*      var card = await this._unitOfWork.Card.GetCard(cardID);
                if(card.CardType.Type == "MonsterCard")
                {
                    throw new Exception("You can't return Monster card with this card.");
                }*/
                await this._playerService.TakeCardFromGraveToHand(grave, playerID, cardID);
            }

            await this._gameService.RemoveCardFromFieldToGrave(fieldID, gameID, playerID);
        }

        public string WhenCanYouActivateTrapCard()
        {
            throw new NotImplementedException();
        }
    }
}
