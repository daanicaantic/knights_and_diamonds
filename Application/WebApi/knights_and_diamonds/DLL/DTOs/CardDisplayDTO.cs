using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    //ovo koristimo kada prikazujemo kartu
    public class CardDisplayDTO
    {
        public string? CardName { get; set; }
        public string CardType { get; set; }
        public int NumberOfCardsAffected { get; set; }
        public int PointsAddedLost { get; set; }
        public string CardEffect { get; set; }
        public int CardLevel { get; set; }
        public int AttackPoints { get; set; }
        public int DefencePoints { get; set; }
        public string? ImgPath { get; set; }
        public string? Description { get; set; }

        public CardDisplayDTO() { }

        public CardDisplayDTO(string? cardName, string cardType, int numberOfCardsAffected, int pointsAddedLost, string cardEffect, int cardLevel, int attackPoints, int defencePoints, string? imgPath, string? description)
        {
            CardName = cardName;
            CardType = cardType;
            NumberOfCardsAffected = numberOfCardsAffected;
            PointsAddedLost = pointsAddedLost;
            CardEffect = cardEffect;
            CardLevel = cardLevel;
            AttackPoints = attackPoints;
            DefencePoints = defencePoints;
            ImgPath = imgPath;
            Description = description;
        }
    }
}
