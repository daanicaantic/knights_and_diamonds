using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable
namespace DAL.DTOs
{
    //ovo koristimo kada prikazujemo kartu
    public class MappedCard
    {
		public int ID { get; set; }
		public string? CardName { get; set; }
		public string? ImgPath { get; set; }
		public string? Description { get; set; }
		public string CardType { get; set; }
		public int CardLevel { get; set; }
		public int AttackPoints { get; set; }
		public int DefencePoints { get; set; }
		public int? NumberOfCardsAffected { get; set; }
        public int? PointsAddedLost { get; set; }
        public int? CardEffectID { get; set; }

        public MappedCard() { }

        public MappedCard(int id,string? cardName, string cardType, int? numberOfCardsAffected, int? pointsAddedLost, int? cardEffectID, int cardLevel, int attackPoints, int defencePoints, string? imgPath, string? description)
        {
			ID = id;
            CardName = cardName;
            CardType = cardType;
            NumberOfCardsAffected = numberOfCardsAffected;
            PointsAddedLost = pointsAddedLost;
            CardEffectID = cardEffectID;
            CardLevel = cardLevel;
            AttackPoints = attackPoints;
            DefencePoints = defencePoints;
            ImgPath = imgPath;
            Description = description;
        }

		public MappedCard(int id,string? cardName, string cardType, int? numberOfCardsAffected, int? pointsAddedLost, int? cardEffectID, string? imgPath, string? description)
		{
			ID = id;
			CardName = cardName;
			CardType = cardType;
			NumberOfCardsAffected = numberOfCardsAffected;
			PointsAddedLost = pointsAddedLost;
			CardEffectID = cardEffectID;
			CardLevel = 0;
			AttackPoints = 0;
			DefencePoints = 0;
			ImgPath = imgPath;
			Description = description;
		}
		public bool CheckCard()
		{
			if (!ImgPath.Contains("Resources/Images/"))
				return false;
			return true;
		}
	}
}
