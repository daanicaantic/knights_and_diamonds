using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class CardService : ICardService
	{
		private readonly KnightsAndDiamondsContext _context;
		public UnitOfWork unitOfWork { get; set; }
		public CardService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
		}
		public async Task<Card> GetCard(int id)
		{
			try
			{
				return await this.unitOfWork.Card.GetOne(id);
			}
			catch
			{
				throw;
			}
		}
		public void AddCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Add(card);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}
		public void RemoveCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Delete(card);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}
		public void UpdateCard(Card card)
		{
			try
			{
				this.unitOfWork.Card.Update(card);
				this.unitOfWork.Complete();
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<Card> FindCardByName(string name)
		{
			try
			{
				return this.unitOfWork.Card.Find(x => x.CardName == name);
			}
			catch
			{
				throw;
			}
		}
	}
}
