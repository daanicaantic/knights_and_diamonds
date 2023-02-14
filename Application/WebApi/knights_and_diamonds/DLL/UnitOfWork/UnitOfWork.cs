using DAL.DataContext;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly KnightsAndDiamondsContext _context;

		public UnitOfWork(KnightsAndDiamondsContext context)
		{
			_context = context;
			Card = new CardRepository(_context);
            Deck = new DeckRepository(_context);
			User = new UserRepository(_context);
			CardInDeck = new CardInDeckRepository(_context);
			RPSGame = new RPSGameRepository(_context);
			PreGame = new PreGameRepository(_context);


		}

		public ICardRepository Card { get; private set; }

        public IDeckRepository Deck { get; private set; }

		public IUserRepository User { get; private set; }

		public ICardInDeckRepository CardInDeck { get; private set; }

		public IRPSGameRepository RPSGame { get; private set; }

		public IPreGameRepository PreGame { get; private set; }

		public int Complete()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
