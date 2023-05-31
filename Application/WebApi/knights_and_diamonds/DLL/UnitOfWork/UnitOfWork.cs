using DAL.DataContext;
using DAL.Models;
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
			Player = new PlayerRepository(_context);
			Game = new GameRepository(_context);
			Effect = new EffectRepository(_context);
			Turn = new TurnRepository(_context);
			CardField = new CardFieldRepository(_context);
			Grave = new GraveRepository(_context);
		}

		public ICardRepository Card { get; private set; }

        public IDeckRepository Deck { get; private set; }

		public IUserRepository User { get; private set; }

		public ICardInDeckRepository CardInDeck { get; private set; }

		public IRPSGameRepository RPSGame { get; private set; }

		public IPlayerRepository Player { get; private set; }

        public IGameRepository Game { get; private set; }

		public IEffectRepository Effect { get; set; }

		public ITurnRepository Turn { get; set; }

		public ICardFieldRepository CardField { get; set; }
		public IGraveRepository Grave { get; set; }




		public async Task Complete()
		{
			await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
