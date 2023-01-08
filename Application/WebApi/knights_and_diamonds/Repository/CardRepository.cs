using Contracts;
using Entities;
using Entities.Models;
using System.Security.Principal;

namespace Repository
{
	public class CardRepository : RepositoryBase<Card>, ICardRepository
	{
		public CardRepository(KnightsAndDiamondsContext repositoryContext)
			: base(repositoryContext)
		{
		}
	}
}