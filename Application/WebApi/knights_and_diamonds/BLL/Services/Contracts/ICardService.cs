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
		Task UpdateCard(UpdateCardDTO card);
		Task RemoveCard(int cardID);
		IQueryable<Card> FindCardByName(string name);
		string SplitType(string effectType);
		Task<List<MappedCard>> GetAllCards();
		Task<MappedCard> MapCard(CardInDeck cardInDeck);
		Task<List<MappedCard>> MapCards(List<CardInDeck> cardsInDeck);
		Task<object> GetFillteredAndOrderedCards(string typleFilter, string sortOrder, string nameFilter, int pageNumber, int pageSize);

	}
}
