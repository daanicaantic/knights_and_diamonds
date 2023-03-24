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
        public async Task AddUser(UserDTO u)
        {
            var userFound = await this.unitOfWork.User.GetUserByEmail(u.Email);
            if(userFound != null)
            {
                throw new Exception("User with this email already exists.");
            }

            userFound = await this.unitOfWork.User.GetUserByUsername(u.UserName);
            if(userFound != null )
            {
                throw new Exception("User with this username already exists.");
            }

            var user = new User(u.Name, u.SurName, u.Email, u.Password, u.UserName, u.Role);

            await this.unitOfWork.User.Add(user);
            this.unitOfWork.Complete();
        }
        public async Task<User> GetUserByID(int id)
        {
            return await this.unitOfWork.User.GetOne(id);
        }

        public IQueryable<User> GetUser(string email, string password)
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

        public async Task<User> SetMainDeckID(int userID, int deckID)
        {

            var deck = await this.unitOfWork.Deck.GetCardsFromDeck(userID, deckID);
            if (deck == null)
            {
                throw new Exception("This user doesn't contains deck with this ID");
            }
            var user = await this.unitOfWork.User.SetMainDeck(userID, deckID);
            this.unitOfWork.Complete();
            return user;

        }
    }
}
