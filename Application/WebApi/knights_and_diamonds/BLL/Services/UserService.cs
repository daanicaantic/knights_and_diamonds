using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork unitOfWork { get; set; }
        public UserService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            unitOfWork = new UnitOfWork(_context);
        }
        public void AddUser(UserDTO u)
        {
            try
            {
                var user = new User(u.Name,u.SurName,u.Email,u.Password,u.UserName,u.Role);
                
                this.unitOfWork.User.Add(user);
                this.unitOfWork.Complete();
            }
            catch
            {
                throw;
            }
        }
        public User GetUserByID(int id)
        {
            return(this.unitOfWork.User.GetOne(id));
        }

        public IEnumerable<User> GetUser(string email, string password)
        {
            try
            {
                return this.unitOfWork.User.Find(x => x.Email == email && x.Password == password);
            }
            catch
            {
                throw;
            }
        }
    }
}
