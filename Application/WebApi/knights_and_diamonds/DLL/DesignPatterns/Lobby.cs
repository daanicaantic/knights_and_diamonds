using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns
{
	public class Lobby
	{
		public int ID { get; set; }
		public OnlineUserDto ?User1 { get; set; }
		public OnlineUserDto ?User2 { get; set; }
		public Lobby() 
		{ 
		
		}
		public Lobby(int iD, OnlineUserDto user1, OnlineUserDto user2)
		{
			ID = iD;
			User1 = user1;
			User2 = user2;
		}
	}
}
