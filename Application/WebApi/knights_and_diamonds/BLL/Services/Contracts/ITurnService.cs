using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public struct TurnInfo
	{
		public int PlayerOnTurn { get; set; }
		public int TurnPhase { get; set; }
	}
	public enum TurnPhase
	{
		DrawPhase,
		MainPhase,
		BeatlePhase,
		EndPhase,
	}
	public interface ITurnService
	{
		Task<TurnInfo> NewTurn(int gameID);
		Task<TurnInfo> GetTurnInfo(int gameID, int playerID);
		Task<TurnPhase> GetTurnPhase(Game game);
	}
}
