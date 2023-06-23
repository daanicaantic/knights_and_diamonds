using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{

	public interface IGameService
	{

		struct GraveToDisplay
		{
			public int GraveCount { get; set; }
			public MappedCard? LastCard { get; set; }
		}
		struct AffterPlaySpellTrapCardData
		{
			public int areaOfClicking { get; set; }
			public int fieldID { get; set; }
		}
		Task<Game> GetGameByID(int gameID);
		Task<GameDTO> GetGame(int gameID, int userID);
		Task<List<string>> GameGroup(int gameID);
		Task<GraveToDisplay> GetGamesGrave(int gameID);
		Task<ConnectionsPerUser> GameConnectionsPerPlayer(int gameID, int playerID);
		Task<FieldDTO> GetPlayersField(int playerID);
		EnemiesFieldDTO GetEneiesField(FieldDTO enemiesField);
		Task<int> NormalSummon(int gameID, int playerID, int cardID, bool position);
		Task<FieldDTO> TributeSummon(List<int> fieldsIDs, int gameID, int playerID, int cardInDeckID, int numberOfStars, bool position);
		Task<AffterPlaySpellTrapCardData> PlaySpellCard(int gameID, int playerID, int cardInDeckID, int cardEffectID);
		Task PlayTrapCard(int gameID, int playerID, int cardInDeckID, int cardEffectID);
		Task ExecuteEffect(List<int> listOfCards, int cardFieldID, int playerID, int gameID);
		Task RemoveCardFromFieldToGrave(int fieldID, int gameID, int playerID);
		Task RemoveCardFromHandToGrave(int playerID, int cardID, int gameID);
		Task<int> AttackEnemiesField(int attackingFieldID, int attackedFieldID, int playerID, int enemiesPlayerID, int gameID);
		Task<List<int>> CanEnemieActivateTrapCard(string activationString, int playerID, int gameID);
		Task<AffterPlaySpellTrapCardData> ActiveTrapCard(int gameID, int playerID, int fieldID);
		Task<int> SetWinner(int playerID, int gameID);
		Task ChangeMonsterPosition(int playerID, int fieldID, int gameID);

    }
}
