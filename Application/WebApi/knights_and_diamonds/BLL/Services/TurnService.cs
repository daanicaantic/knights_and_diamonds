using BLL.Factory;
using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns;
using DAL.DesignPatterns.Factory;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class TurnService:ITurnService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork _unitOfWork { get; set; }
		public TurnService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			_unitOfWork = new UnitOfWork(_context);
		}

		public async Task<int> GetCurrentPhase(int turnID,int gameID)
		{
			var game = await this._unitOfWork.Game.GetGameWithTurns(turnID);
			var turn = game.Turns.Find(x=>x.ID==turnID);
			var turnPhase = -1;//0-dp,1-mp,2-bp,3-ep //-1 error
			if (turn == null)
			{
				throw new Exception("Turn with this ID is not in this game");
			}
			if (turn.DrawPhase == true)
			{
				turnPhase = 0;
			}
			else if (turn.MainPhase==true)
			{
				turnPhase = 1;
			}
			else if (turn.BattlePhase == true)
			{
				turnPhase = 2;
			}
			else if (turn.EndPhase == true)
			{
				turnPhase = 3;
			}
			return turnPhase;
		}
	}
}
