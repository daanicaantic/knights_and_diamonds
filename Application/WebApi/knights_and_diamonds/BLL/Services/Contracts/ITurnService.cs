using DAL.DTOs;
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
		public bool IsMonsterSummoned { get; set; }
		public int TurnNumber { get;set; }
	}
	public struct Phases
	{
		public int Key { get; set; }
		public string Name { get; set; }
		public bool Status { get; set; }
		public Phases(int key, string name, bool status)
		{
			this.Key = key;
			this.Name = name;
			this.Status = status;
		}
	}
	public enum TurnPhase
	{
		DrawPhase,
		MainPhase,
		BeatlePhase,
		EndPhase,
		NoPhase,
	}
	public interface ITurnService
	{
		Task<Turn> NewTurn(Game game);
		Task<TurnInfo> GetTurnInfo(int gameID, int playerID);
		TurnPhase GetTurnPhase(Game game);
		Task<List<int>> BattlePhase(int gameID, int playerID);
		Task<List<MappedCard>> DrawPhase(int gameID, int playerID);
		Task<Game> EndPhase(int gameID, int playerID, int enemiesID);
		Task ChangeToMainPhase(int gameID);

	}
}
