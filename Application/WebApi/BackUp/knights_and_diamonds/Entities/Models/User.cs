using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	enum Role{
		Admin,
		User,
		Guest
	}
	public class User
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string SurName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string UserName { get; set; }
		public string Role { get; set; }
	}
}
