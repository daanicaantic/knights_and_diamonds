using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IPlayerService
    {
		Task<Player> GetPlayer(int playerID);
		Task<List<CardInDeck>> SetPlayersDeck(int userID);
		Task<List<MappedCard>> GetPlayersHand(int playerID);
		Task<int> GetNumberOfCardsInDeck(int playerID);
		Task<MappedCard> Draw(int playerID);
		Task StartingDrawing(int playerID);
		Task SetGameStarted(Player player);
		Task<List<CardOnFieldDisplay>> GetPlayersCardFields(int playerID);
		Task TakeCardFromEnemiesHand(PlayersHand enemiesHand, int playerID, int cardID);
		Task TakeCardFromGraveToHand(Grave grave, int playerID, int cardID);
		void TakeCardFromGraveToField(Grave grave, CardField cardField, int cardID);
		Task SetFieldPosition(int playerID, bool position);
    }
}
