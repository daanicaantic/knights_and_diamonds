using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }
		public int PlayerOnTurn { get; set; }
		public int TurnNumber { get; set; }
        [JsonIgnore]
        public List<Player>? Players { get; set; }
		[JsonIgnore]
		public List<Turn>? Turns { get; set; }
        public int? GraveID { get; set; }
        public Grave? Grave { get; set; }
        public int Winner { get; set; }
		public int Loser { get; set; }

		public Game()
        {
            Grave = new Grave();
            GraveID=Grave.ID;
        }
        public void NewTurn(Turn turn)
        {
            this.Turns.Add(turn);
            this.TurnNumber = turn.ID;
        }

    }
}
