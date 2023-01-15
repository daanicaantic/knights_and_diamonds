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
            get { return Context as KnightsAndDiamondsContext; }
        }
    }
}
