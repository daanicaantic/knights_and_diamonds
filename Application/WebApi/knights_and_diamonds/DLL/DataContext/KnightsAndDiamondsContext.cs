﻿using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class KnightsAndDiamondsContext : DbContext
    {
        public KnightsAndDiamondsContext(DbContextOptions options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
		public DbSet<Deck> Decks { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserHand> Hands { get; set; }



	}
}