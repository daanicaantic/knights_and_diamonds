using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public abstract class Effect
	{
		public int ID { get; set; }
		public string EffectText { get; set; }
		public abstract void DoEffect(Table table,int numberofcards);
	}
}
