﻿using BLL.Strategy.Context;
using DAL.DataContext;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Strategy.Context.IEffectExecution;

namespace BLL.Strategy
{
	public class TakeCardFromEnemiesHandExecution : IEffectExecution
	{
		private readonly KnightsAndDiamondsContext _context;

		public TakeCardFromEnemiesHandExecution(KnightsAndDiamondsContext context)
		{
			this._context = context;
		}

		public ChooseCardsFrom SelectCardsFrom()
		{
			Console.WriteLine("Enemies hanad");
			return ChooseCardsFrom.EnemiesHand;
		}
		public Task ExecuteEffect(List<int> listOfCards, int effectID)
		{
			throw new NotImplementedException();
		}

		public Task ExecuteEffect(List<int> listOfCards, Effect effect, int playerID, int gameID, int fieldID)
		{
			throw new NotImplementedException();
		}
	}
}
