using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DesignPatterns.Factory;
using DAL.DesignPatterns.Factory.Contract;
using DAL.DTOs;
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
		public IDescriptionFactory _descriptionFactory { get; set; }
		public IEffectService _effectService { get; set; }

		public IFactory _factory { get; set; }
		public CardService(KnightsAndDiamondsContext context)
		{
			this._context = context;
			unitOfWork = new UnitOfWork(_context);
			_descriptionFactory = new ConcreteDescriptionFactory();
			_effectService = new EffectService(_context);
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
		public async Task<Card> AddCard(CardDTO cardDTO)
		{
			try
			{
				var effect=await this._effectService.AddEffect(cardDTO.EffectTypeID, cardDTO.NumOfCardsAffected, cardDTO.PointsAddedLost);
				var card = new Card(cardDTO.CardName, cardDTO.ImgPath, cardDTO.CardTypeID, cardDTO.EffectTypeID, effect);

				var c=await this.unitOfWork.Card.AddCard(card);
				this.unitOfWork.Complete();
				return c;
			}
			catch(Exception e)
			{
				throw e;
			}
		}
		public async Task<MonsterCard> AddMonsterCard(MonsterCard card)
		{
			try
			{
				var c = await this.unitOfWork.Card.AddMonsterCard(card);
				this.unitOfWork.Complete();
				return c;
			}
			catch (Exception e)
			{
				throw e;
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

		public IQueryable<Card> FindCardByName(string name)
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
