using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubConfig
{
	public sealed class ConnectionsHub
	{
		private ConnectionsHub()
		{
			ConnectedUsers = new List<int>();
		}

		private static readonly object _lock = new object();
		private static ConnectionsHub? _instance = null;

		public static ConnectionsHub GetInstance()
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new ConnectionsHub();
					}
				}
			}
			return _instance;
		}
		public List<int> ConnectedUsers { get; set; }


	}
}
