using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory
{
	public class NoEffect:IFactory
	{
		public string Description { get; set; }
		public Effect Effect { get; set; }
		public NoEffect()
		{
			this.Effect = new Effect();
			this.SetDescription();
			this.SetEffect();
		}
		public string SetDescription()
		{
			return this.Description = "This card has no Effect";
		}
		public string GetDescription()
		{
			throw new NotImplementedException();
		}
		public void SetEffect()
		{
			this.Effect.Description = this.Description;
		}
		public Effect GetEffect()
		{
			return this.Effect;
		}
	}
}
