using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class KnightsAndDiamondsContext : DbContext
    {
        public KnightsAndDiamondsContext(DbContextOptions options) : base(options) { }

        public DbSet<Card>? Cards { get; set; }
		public DbSet<MonsterCard>? MonsterCards { get; set; }
		public DbSet<CardType>? CardTypes { get; set; }
		public DbSet<ElementType>? ElementTypes { get; set; }
		public DbSet<MonsterType>? MonsterTypes { get; set; }
		public DbSet<Deck>? Decks { get; set; }
		public DbSet<User>? Users { get; set; }
		public DbSet<CardInDeck>? CardInDecks { get; set; }
		public DbSet<RockPaperScissorsGame>? RockPaperScissorsGames { get; set; }
		public DbSet<Player>? Players { get; set; }
		public DbSet<Game>? Games { get; set; }
		public DbSet<Effect>? Effects { get; set; }
		public DbSet<EffectType>? EffectTypes { get; set; }
		public DbSet<PlayersHand>? PlayerHands { get; set; }
		public DbSet<Turn>? Turns { get; set; }

	}
}
