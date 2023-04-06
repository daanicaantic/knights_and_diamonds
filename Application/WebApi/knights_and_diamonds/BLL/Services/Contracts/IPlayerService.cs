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
		Task<List<CardDisplayDTO>> GetPlayersHand(int playerID);
		Task<int> GetNumberOfCardsInDeck(int playerID);
		Task<CardDisplayDTO> Draw(int playerID);

	}
}
