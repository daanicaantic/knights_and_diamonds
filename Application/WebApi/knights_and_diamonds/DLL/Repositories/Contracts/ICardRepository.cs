using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface ICardRepository : IRepository<Card>
	{
		public struct FilteredCards
		{
			public int TotalItems { get; set; }
			public int TotalPages { get; set; }
			public int PageNumber { get; set; }
			public int PageSize { get; set; }
			public List<Card> Cards { get; set; }
		}
		public Task<Card> AddCard(Card card);
		public Task<MonsterCard> AddMonsterCard(MonsterCard card);
		MonsterCard UpdateMonsterCard(MonsterCard card);
		Task<CardType> GetCardType(int cardTypeID);
		Task<List<Card>> GetSpellTrapCards();
		Task<List<MonsterCard>> GetMonsterCards();
		Task<Card> GetCard(int cardID);
		Task<CardType> GetCardTypeByName(string type);
		Task<MonsterCard> GetMonsterCard(int cardID);
		Task<List<CardType>> GetCardTypes();
		Task<Card> GetCardAndCardsInDeck(int cardID);
		Task<FilteredCards> FilterAndOrderCards(string typleFilter, string sortOrder, string nameFilter, int pageNumber, int pageSize);



	}
}
