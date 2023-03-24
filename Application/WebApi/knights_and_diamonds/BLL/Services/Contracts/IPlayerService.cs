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
        Task<List<CardInDeck>> SetPlayersDeck(int userID);
		Task<List<Card>> GetPlayersHand(int playerID);
		Task<int> GetNumberOfCardsInDeck(int playerID);
		Task<Card> Draw(int playerID);

	}
}
