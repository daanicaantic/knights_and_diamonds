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
		IQueryable<Card> GetCardsPerPage(int pageIndex, int pageSize);
		public Task<Card> AddCard(Card card);
		public Task<MonsterCard> AddMonsterCard(MonsterCard card);
		Task<CardType> GetCardType(int cardTypeID);
		Task<Card> GetCardByName(string cardName);

	}
}
