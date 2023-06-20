using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class UserDTO
    {   
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
		public UserDTO(string name, string surname,string email,string password,string role, string username)
		{
			this.Name = name;
			this.SurName = surname;
			this.UserName = username;
            this.Email = email;
            this.Password = password;
            this.Role = role;
            this.UserName = username;
		}
	}
    public class OnlineUserDto 
    {
        public int ID { get; set; }
		public string? Name { get; set; }
		public string? SurName { get; set; }
		public string? UserName { get; set; }

        public OnlineUserDto(int id, string name, string surname, string username) 
        {
            this.ID = id;
            this.Name = name;
			this.SurName = surname;
			this.UserName = username;
        }
	}
}
