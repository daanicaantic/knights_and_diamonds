using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubConfig
{
	public class MyHub:Hub
	{
		public async Task askServer(string someTextForClient) 
		{
			string tempstring;
			if(someTextForClient == "hey") 
			{
				tempstring = "HI";
			}
			else 
			{
				tempstring = "NEHI";
			}
			await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse",tempstring);
		}
	}
}
