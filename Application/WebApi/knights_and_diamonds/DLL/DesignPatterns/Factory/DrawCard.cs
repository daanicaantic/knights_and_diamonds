using DAL.DesignPatterns.Factory.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DesignPatterns.Factory
{
    public class DrawCard : IFactory
    {
        public string description { get; set; }

        public DrawCard(int numOfCardAffected) 
        {
            this.SetDescription(numOfCardAffected);
        }
        public void SetDescription(int numOfCardAffected)
        {
            this.description = "Draw " + numOfCardAffected.ToString() + " card from your deck.";
        }

        public async Task<string> GetDescription()
        {
            return this.description;
        }
    }
}
