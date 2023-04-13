using BLL.Services.Contracts;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BLL.Services.Contracts.IGameService;

namespace BLL.Services
{
    public struct ConnectionsPerUser
    {
        public List<string> MyConnections { get; set; }
		public List<string> EnemiesConnections { get; set; }
	}
	public class GameService : IGameService
    {
        private readonly KnightsAndDiamondsContext _context;
        public UnitOfWork _unitOfWork { get; set; }
        public IDeckService _deckService { get; set; }
        public IConnectionService _connectionService { get; set; }
		public ICardService _cardService { get; set; }
		public IPlayerService _playerService { get; set; }

		public GameService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            this._unitOfWork = new UnitOfWork(_context);
            this._deckService = new DeckService(_context);
			this._connectionService = new ConnectionService(_context);
            this._cardService = new CardService(_context);
            this._playerService = new PlayerService(_context);
		}

		public async Task<List<string>> GameGroup(int gameID)
        {
			var group = new List<string>();
			var game = await this._unitOfWork.Game.GetGameWithPlayers(gameID);
            if (game == null)
            {
                throw new Exception("There is no game with this ID");
            }
            foreach (var player in game.Players)
            {
				var connections = await this._connectionService.GetConnectionByUser(player.UserID);
                group.Concat(connections);
			}
            return group;
        }
		public async Task<ConnectionsPerUser> GameConnectionsPerPlayer(int gameID,int playerID)
		{
            var connectionsPerUser = new ConnectionsPerUser();
			var game = await this._unitOfWork.Game.GetGameWithPlayers(gameID);
			if (game == null)
            {
                throw new Exception("There is no game with this ID");
            }
			foreach (var player in game.Players)
			{
                    var connections = await this._connectionService.GetConnectionByUser(player.UserID);
                    if (player.ID == playerID)
                    {
                        connectionsPerUser.MyConnections = connections;
                    }
                    else
                    {
                        connectionsPerUser.EnemiesConnections = connections;
                    }
			}
			return connectionsPerUser;
		}
        
        public async Task<Game> GetGameByID(int gameID)
        {
            var game = await this._unitOfWork.Game.GetOne(gameID);
            if (game == null)
            {
                throw new Exception("There is no game with this ID");
            }
            return game;
        }
    /*    public async Task SetGameStarted(Game game)
        {
            game.GameStarted = game.GameStarted+1;
            this._unitOfWork.Game.Update(game);
            this._unitOfWork.Complete();
        }*/
		public async Task<GameDTO> GetGame(int gameID, int userID)
		{
			GameDTO game = new GameDTO();
			var gaame = await this._unitOfWork.Game.GetGameWithPlayers(gameID);
			foreach (var player in gaame.Players)
			{
				if (player.UserID == userID)
				{
					game.PlayerID = player.ID;
					game.GameID = player.GameID;
				}
				else
				{
					game.EnemiePlayerID = player.ID;
				}
			}
			game.GameID = gaame.ID;
			return game;
		}

        public async Task<FieldDTO> GetPlayersField(int playerID)
        {
			var field = new FieldDTO();
            var player = await this._unitOfWork.Player.GetPlayersField(playerID);
            if (player == null)
            {
                throw new Exception("There is no player with this id");
            }
			//true-vec je pocela partija(tj vec je vukao pocetne karte)//false=!true
			field.GameStarted = player.GaemeStarted;
            field.LifePoints=player.LifePoints;
            field.DeckCount = player.Deck.Count;
			field.Hand = await this._cardService.MapCards(player.Hand.CardsInHand);
 
			var cardsOnField = new List<CardOnFieldDisplay>();
            foreach (var f in player.Fields)
            {
                var cardOnField = new CardOnFieldDisplay();
                if (f.CardOnField != null)
                {
                    var mappedCard = await this._cardService.MapCard(f.CardOnField);
                    cardOnField.CardOnField = mappedCard;
                }
                cardOnField.FieldID = f.ID;
                cardOnField.CardPosition = f.CardPosition;
                cardOnField.CardShowen = f.CardShowen;
                cardOnField.FieldIndex = f.FieldIndex;
                cardsOnField.Add(cardOnField);
            }
            field.CardFields = cardsOnField;
            return field;
        }
		public EnemiesFieldDTO GetEneiesField(FieldDTO enemiesField)
		{
			var field = new EnemiesFieldDTO();
            field.LifePoints = enemiesField.LifePoints;
		    field.DeckCount= enemiesField.DeckCount;
            field.CardFields= enemiesField.CardFields;
            field.HandCount = enemiesField.Hand.Count;
			return field;
		}

		public async Task<GamePhase> GetGamePhase(int gameID)
        {
            var game = await this._unitOfWork.Game.GetOne(gameID);
            if (game == null)
            {
                throw new Exception("There is no game with this ID");
            }
            if (game.TurnNumber == 0)
            {
                return GamePhase.DrawPhase;
            }
            var turn = await this._unitOfWork.Turn.GetOne(game.TurnNumber);
            if (turn == null)
            {
                throw new Exception("There is no turn with this ID");
			}
            if (turn.DrawPhase)
            {
				return GamePhase.DrawPhase;
			}
            else if (turn.MainPhase)
            {
				return GamePhase.MainPhase;
			}
			else if (turn.BattlePhase)
			{
				return GamePhase.BeatlePhase;
			}
			else if (turn.EndPhase)
			{
				return GamePhase.EndPhase;
			}
            throw new Exception("There is some error");
		}

        public async Task<int> GetPlayerOnTurn(int gameID)
        {
			var game = await this._unitOfWork.Game.GetOne(gameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
            if (game.PlayerOnTurn == 0)
            {
                throw new Exception("Error no one is set for turn");
            }
            return game.PlayerOnTurn;
		}

		public async Task<Game> NewTurn(int gameID)
        {
            var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
          /*  if (game.TurnNumber != 0)
            {
                var lastturn = game.Turns.Last();
                if (lastturn.EndPhase == false)
                {
                    throw new Exception("Last turn is not finnished yet");
                }
            }*/
            var turn = new Turn();
            game.Turns.Add(turn);
			game.TurnNumber = game.Turns.Count();
            this._unitOfWork.Game.Update(game);
            await this._unitOfWork.Complete();
            return game;
		}
        public async Task<Turn> GetTurn(int gameID)
        {
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
            if (game.TurnNumber == 0)
            {
				throw new Exception("There is still no turns in this game");
			}
            var turn = game.Turns.Last();
            return turn;
		}
		public async Task<List<MappedCard>> DrawPhase(int gameID)
        {
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			if (game == null)
			{
				throw new Exception("There is no game with this ID");
			}
            var turn = game.Turns.Last();
            if (turn.DrawPhase == false) 
            {
                throw new Exception("You cant draw card out of draw phase");
            }
            var player = await this._unitOfWork.Player.GetPlayerWithHandAndDeckByID(game.PlayerOnTurn);
            if (player == null)
            {
				throw new Exception("There is no player with this ID");
			}

			var card = await this._playerService.Draw(game.PlayerOnTurn);
			if (player.Hand.CardsInHand.Count >= 7)
			{
                throw new Exception("Throw " + (player.Hand.CardsInHand.Count - 6).ToString() + " cards from your hand");
			}
            turn.DrawPhase = false;
            turn.MainPhase = true;
            this._unitOfWork.Turn.Update(turn);
            await this._unitOfWork.Complete();

            var hand=await this._cardService.MapCards(player.Hand.CardsInHand);
            return hand;
		}

		public async Task<CardField> NormalSummon(int gameID,int playerID,int cardID,bool position)
        {
            var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
            if (game == null) 
            { 
                throw new Exception("There is no game with this ID");
			}
            if (game.Turns.Count == 0)
            {
				throw new Exception("This game has no turns");
			}
			var turn = game.Turns.Last();
            if (game.Turns.Last().MainPhase==false)
            {
				throw new Exception("You can play card only in main phase");
			}
            if (turn.MonsterSummoned == true)
            {
                throw new Exception("You already summon monster in this turn");
            }
			if (game.PlayerOnTurn != playerID)
            {
				throw new Exception("It is not your turn to play");
			}
			var player = await this._unitOfWork.Player.GetPlayerWithFields(playerID);
            if (player == null)
            {
                throw new Exception("There is no player with this ID");
            }
            var cardToBePlayed = await this._unitOfWork.CardInDeck.GetOne(cardID);
            var emptyField=player.Fields.Where(x => x.CardOnField == null && x.FieldType == "MonsterField").FirstOrDefault();
            emptyField.CardOnField = cardToBePlayed;
            emptyField.CardPosition = position;
            if (position)
            {
                emptyField.CardShowen = true;
            }
            else
            {
				emptyField.CardShowen = false;
			}
            turn.MonsterSummoned = true;
			this._unitOfWork.Turn.Update(turn);
			this._unitOfWork.CardField.Update(emptyField);
            await this._unitOfWork.Complete();
            return emptyField;
		}
/*        public async Task<CardField> SpetialSummon(int gameID, int playerID, int cardID, bool position,int numberOfStars)
        {

        }*/
    }
}
