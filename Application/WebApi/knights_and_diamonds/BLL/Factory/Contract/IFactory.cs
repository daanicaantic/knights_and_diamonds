using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory.Contract
{
	public interface IFactory
	{	
		string GetDescription();
		Effect GetEffect();
	}
}
