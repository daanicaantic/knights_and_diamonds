using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
	public interface ICardService
	{
		Task<Card> GetCard(int id);
		void AddCard(Card card);
		void RemoveCard(Card card);
		void UpdateCard(Card card);
		IEnumerable<Card> FindCardByName(string name);
	}
}
