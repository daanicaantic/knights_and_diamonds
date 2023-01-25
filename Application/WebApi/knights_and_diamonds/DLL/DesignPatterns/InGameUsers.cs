using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns
{
	public sealed class UserInGameSinglton
	{
		private UserInGameSinglton() {
			UsersInGame = new List<int>();
		}

		private static readonly object _lock = new object();
		private static UserInGameSinglton? _instance=null;

		public static UserInGameSinglton GetInstance()
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new UserInGameSinglton();
					}
				}
			}
			return _instance;
		}

		public List<int> UsersInGame { get; set; }
	}
}
