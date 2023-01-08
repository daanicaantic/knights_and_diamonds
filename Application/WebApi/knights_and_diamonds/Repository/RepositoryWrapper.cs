using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
	public class RepositoryWrapper : IRepositoryWrapper
	{
		private KnightsAndDiamondsContext _repoContext;
		private ICardRepository _card;

		public ICardRepository Card
		{
			get
			{
				if (_card == null)
				{
					_card = new CardRepository(_repoContext);
				}

				return _card;
			}
		}
		public ICardRepository MonsterCard
		{
			get
			{
				if (_monstercard == null)
				{
					_card = new CardRepository(_repoContext);
				}

				return _card;
			}
		}
		public RepositoryWrapper(KnightsAndDiamondsContext repositoryContext)
		{
			_repoContext = repositoryContext;
		}

		public void Save()
		{
			_repoContext.SaveChanges();
		}
	}
}