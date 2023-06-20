using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTOs
{
	//ovo koristimo kada dodajemo kartu
	public class CardDTO
	{
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public int CardTypeID { get; set; }
		public int EffectTypeID { get; set; }
		public int NumOfCardsAffected { get; set; }
		public int PointsAddedLost { get; set; }
		public int CardLevel { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }

		public CardDTO() { }
    }
	public class UpdateCardDTO
	{
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public int CardLevel { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }

		public UpdateCardDTO() { }
	}

}
