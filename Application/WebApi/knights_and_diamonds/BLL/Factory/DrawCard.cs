using DAL.DesignPatterns.Factory.Contract;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
    public class DrawCard : IFactory
    {
        public string Description { get; set; }
        public Effect Effect { get; set; }

        public DrawCard(int numberOfCardsToDraw) 
        {
            this.Effect = new Effect();
            this.SetDescription(numberOfCardsToDraw);
            this.SetEffect(numberOfCardsToDraw);
        }

        public void SetDescription(int numberOfCardsToDraw)
        {
            this.Description = "Draw " + numberOfCardsToDraw.ToString() + " card(s) from your deck.";
        }

        public string GetDescription()
        {
            return this.Description;
        }

        public void SetEffect(int numberOfCardsToDraw)
        {
            this.Effect.PointsAddedLost = numberOfCardsToDraw;
            this.Effect.Description = this.Description;
        }

        public Effect GetEffect()
        {
            return this.Effect;
        }
    }
}
