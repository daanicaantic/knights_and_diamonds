using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class KnightsAndDiamondsContext : DbContext
    {
        public KnightsAndDiamondsContext(DbContextOptions options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<MonsterCard> MonsterCards { get; set; }
        public DbSet<SpellTrapCard> SpellTrapCards { get; set; }

    }
}
