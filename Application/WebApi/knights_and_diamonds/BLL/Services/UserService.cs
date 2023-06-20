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
        public UnitOfWork _unitOfWork { get; set; }
        public UserService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            _unitOfWork = new UnitOfWork(_context);
        }
        public async Task AddUser(UserDTO u)
        {
			var userFound = await this._unitOfWork.User.GetUserByEmail(u.Email);
            if(userFound != null)
            {
                throw new Exception("User with this email already exists.");
            }

            userFound = await this._unitOfWork.User.GetUserByUsername(u.UserName);
            if(userFound != null )
            {
                throw new Exception("User with this username already exists.");
            }

            var user = new User(u.Name, u.SurName, u.Email, u.Password, u.UserName, u.Role);

            await this._unitOfWork.User.Add(user);
            await this._unitOfWork.Complete();
        }
        public async Task<User> GetUserByID(int id)
        {
            return await this._unitOfWork.User.GetOne(id);
        }

        public IQueryable<User> GetUser(string email, string password)
        {
            try
            {
                return this._unitOfWork.User.Find(x => x.Email == email && x.Password == password);
            }
            catch
            {
                throw;
            }
        }
    }
}
