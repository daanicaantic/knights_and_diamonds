using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork unitOfWork { get; set; }
        public GameService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            unitOfWork = new UnitOfWork(_context);
        }

        public async Task<int> StartGame(int player1ID, int player2ID)
        {
            var player1 = await this.unitOfWork.Player.GetOne(player1ID);

            if(player1 == null)
            {
                throw new Exception("Player1 is undefined.");
            }

            var player2 = await this.unitOfWork.Player.GetOne(player2ID);

            if(player2 == null)
            {
                throw new Exception("Player2 is undefined.");
            }

            Game game = new Game();
            this.unitOfWork.Game.Add(game);

            player1.Game = game;
            player2.Game = game;

            this.unitOfWork.Complete();

            return game.ID;
        }
    }
}
