using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DesignPatterns;
namespace BLL.Services
{
	public class RPSGameService : IRPSGamaeService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		private UserInGameSinglton _usersingame { get; set; }
		public RPSGameService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			_usersingame = UserInGameSinglton.GetInstance();
		}
		public bool NewGame(User user)
		{
			if (!this._usersingame.UsersInGame.Contains(user.ID)) 
			{
				this._usersingame.UsersInGame.Add(user.ID);
				var rpsGame = new RockPaperScissorsGame();
				var player = new PreGameSession();
				player.RPSGame = rpsGame;
				player.User = user;
				this.unitOfWork.PreGame.Add(player);
				this.unitOfWork.Complete();
				return true;
			}
			else
			{
				return false;
			}

		}
	}
}
