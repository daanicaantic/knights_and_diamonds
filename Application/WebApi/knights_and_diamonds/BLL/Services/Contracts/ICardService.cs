using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public interface ICardService
	{
		Task<Card> GetCard(int id);
		Task AddCard(CardDTO card);
		Task RemoveCard(Card card);
		Task UpdateCard(Card card);
		IQueryable<Card> FindCardByName(string name);
		string SplitType(string effectType);
		Task<List<CardDisplayDTO>> GetAllCards();
		Task<CardDisplayDTO> MapCard(CardInDeck cardInDeck);

    }
}
