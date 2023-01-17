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
        void AddUser(UserDTO user);
        User GetUser(int id);
        IEnumerable<User> GetUser(string email, string password);
    }
}
