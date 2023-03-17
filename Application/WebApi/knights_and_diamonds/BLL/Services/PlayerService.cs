using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public PlayerService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _unitOfWork = new UnitOfWork(_context);
        }

        public async Task<List<CardInDeck>> GetShuffledDeck(int playerID)
        {
            var shuffledDeck = await _unitOfWork.Player.GetShuffledDeck(playerID);
            if (shuffledDeck == null)
            {
                throw new Exception("Deck unknown.");
            }
            return shuffledDeck;
        }
    }
}
