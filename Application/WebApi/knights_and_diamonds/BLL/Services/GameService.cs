using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public IDeckService _deckService { get; set; }
        public GameService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            this._unitOfWork = new UnitOfWork(_context);
            this._deckService = new DeckService(_context);
        }

        public async Task SetShuffledDeck(int playerID)
        {
            var player = await _unitOfWork.Player.GetPlayerByID(playerID);
            if (player == null)
            {
                throw new Exception("Player unknown.");
            }

            var shuffledDeck = await this._deckService.ShuffleDeck(player.User.MainDeckID, player.UserID);
            player.Deck = shuffledDeck;

            _unitOfWork.Player.Update(player);
            _unitOfWork.Complete();
        }
    }
}
