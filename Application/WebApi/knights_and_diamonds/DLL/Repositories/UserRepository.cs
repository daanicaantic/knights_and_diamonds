using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

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
            var user = await this.Context.Users?.Where(x => x.Email == email).FirstOrDefaultAsync();
			return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await this.Context.Users?.Where(x => x.UserName == username).FirstOrDefaultAsync();
            return user;
        }
		public async Task<User> FindUserPerMailAndPassword(string email,string password)
		{
			var user = await this.Context.Users?.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
			if (user == null)
			{
				throw new Exception("Wrong email or password");
			}
			return user;
		}

        public async Task<object> WinsAndLosesForUser(int userID)
        {
            var winsCount = await Context?.Games?.Where(x => x.Winner == userID).CountAsync();
            var losesCount = await Context?.Games?.Where(x => x.Loser == userID).CountAsync();

            var result = new
            {
                WinsCount = winsCount,
                LosesCount = losesCount,
            };
            return result;
        }
    }
}
