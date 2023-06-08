using DAL.DataContext;
using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class CardFieldRepository:Repository<CardField>,ICardFieldRepository
	{
		public CardFieldRepository(KnightsAndDiamondsContext context) : base(context)
		{
		}
		public KnightsAndDiamondsContext Context
		{
			get { return _context as KnightsAndDiamondsContext; }
		}

		public async Task<CardField> GetCardField(int fieldID,int playerID)
		{
			var cardField= await this.Context.CardFields?.Where(x => x.ID == fieldID && x.PlayerID==playerID).Include(x => x.CardOnField).ThenInclude(x => x.Card).FirstOrDefaultAsync();
			if (cardField == null)
			{
				throw new Exception("Field with this ID dose not exist");
			}
			return cardField;
			
		}
		/*public async Task<CardField> GetCardFieldWithAttackInTurn (int fieldID, int playerID)
		{
			var cardField = await this.Context.CardFields?.Where(x => x.ID == fieldID && x.PlayerID == playerID).Include(x => x.CardOnField).ThenInclude(x => x.Card).FirstOrDefaultAsync();
			if (cardField == null)
			{
				throw new Exception("Field with this ID dose not exist");
			}
			return cardField;

		}*/
		public async Task<List<CardField>> GetFieldsByPlayerID(int playerID)
		{
			var cardFields = await this.Context.CardFields?.Where(x => x.PlayerID == playerID).Include(x => x.CardOnField).ThenInclude(x => x.Card).ToListAsync();
			if (cardFields == null)
			{
				throw new Exception("There is no fields for this playerID");
			}
			return cardFields;

		}
		public async Task<List<CardField>> GetPlayerFields(int playerID,string fieldType)
		{
			var cardFields = await this.Context.CardFields?.Where(x => x.PlayerID ==playerID && x.FieldType==fieldType ).Include(x => x.CardOnField).ThenInclude(x => x.Card).ToListAsync();
			if (cardFields == null)
			{
				throw new Exception("There is no fields for this playerID");
			}
			return cardFields;

		}
		public async Task<List<CardField>> GetEmptyPlayerFields(int playerID, string fieldType)
		{
			var cardFields = await this.Context.CardFields?.Where(x => x.PlayerID == playerID && x.FieldType == fieldType && x.CardOnField==null).Include(x => x.CardOnField).ToListAsync();
			if (cardFields == null)
			{
				throw new Exception("There is no fields for this playerID");
			}
			return cardFields;
		}
		public async Task<List<CardField>> GetFieldByCardInDeckID (int cardInDeckID)
		{
			var cardField = await this.Context.CardFields?.Where(x => x.CardOnFieldID == cardInDeckID).Include(x => x.CardOnField).ToListAsync();
			return cardField;
		}
	}
}
