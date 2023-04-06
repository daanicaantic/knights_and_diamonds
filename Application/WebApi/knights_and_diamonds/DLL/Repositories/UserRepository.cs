using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(KnightsAndDiamondsContext context) : base(context)
        {

        }
        public KnightsAndDiamondsContext Context
        {
            get { return _context as KnightsAndDiamondsContext; }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await this.Context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await this.Context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> SetMainDeck(int userID, int deckID)
        {
            var user = await this.Context.Users.Where(x => x.ID == userID).FirstOrDefaultAsync();
            user.MainDeckID = deckID;
            this.Context.Users.Update(user);
            return user;
        }
    }
}
