using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface ICardInDeckRepository : IRepository<CardInDeck>
	{
        Task<CardInDeck> RemoveCardFromDeck(int cardID, int deckID);

    }
}
