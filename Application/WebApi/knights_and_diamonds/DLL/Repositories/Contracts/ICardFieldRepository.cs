using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
	public interface ICardFieldRepository:IRepository<CardField>	
	{
		Task<CardField> GetCardField(int fieldID,int playerID);
		Task<List<CardField>> GetPlayerFields(int playerID, string fieldType);
		Task<List<CardField>> GetEmptyPlayerFields(int playerID, string fieldType);
		Task<List<CardField>> GetFieldByCardInDeckID(int cardInDeckID);
		Task<List<CardField>> GetFieldsByPlayerID(int playerID);
	}
}
