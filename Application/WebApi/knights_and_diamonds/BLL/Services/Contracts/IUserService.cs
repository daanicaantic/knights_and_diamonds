using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IUserService
    {
        Task AddUser(UserDTO user);
        Task<User> GetUserByID(int id);
        IQueryable<User> GetUser(string email, string password);
        Task<User> SetMainDeckID(int userID, int deckID);
    }
}
