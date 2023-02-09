using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns
{
	public sealed class InGameUsers
	{
		private InGameUsers() {

			GamesIDs = new Dictionary<int, List<int>>();
			Lobbies = new List<Lobby>();
			UsersInGame = new List<int>();
			lobbyID = 0;

		}

		private static readonly object _lock = new object();
		private static InGameUsers? _instance=null;

		public static InGameUsers GetInstance()
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new InGameUsers();
					}
				}
			}
			return _instance;
		}
		public int lobbyID  { get; set; }
		public List<Lobby> Lobbies { get; set; }
		public Dictionary<int, List<int>> GamesIDs { get; set; }

		public List<int> UsersInGame { get; set; }


	}
}
