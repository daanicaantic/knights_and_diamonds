using BLL.AttackingStrategy;
using BLL.Services.Contracts;
using BLL.Strategy;
using DAL.DataContext;
using DAL.DTOs;
using DAL.Migrations;
using DAL.Models;
using DAL.UnitOfWork;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ConcreteStrategy _concreteStrategy { get; set; }
		public ConcreteAttackingStrategy _concreteAttackingStrategy { get; set; }

		public GameService(KnightsAndDiamondsContext context)
        {
            this._context = context;
            this._unitOfWork = new UnitOfWork(_context);
            this._deckService = new DeckService(_context);
			this._connectionService = new ConnectionService(_context);
            this._cardService = new CardService(_context);
            this._playerService = new PlayerService(_context);
            this._concreteStrategy = new ConcreteStrategy(_context);
			this._concreteAttackingStrategy = new ConcreteAttackingStrategy(_context);

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
                if (connections == null)
                {
					throw new Exception("There is no connections for this player");
				}
                foreach (var con in connections)
                {
                    group.Add(con);
                }
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
        public async Task<GraveToDisplay> GetGamesGrave(int gameID)
        {
            var graveForDisplay = new GraveToDisplay();
            var grave = await this._unitOfWork.Grave.GetGraveByGameID(gameID);
            if (grave.ListOfCardsInGrave.Count == 0)
            {
                graveForDisplay.GraveCount = 0;
                graveForDisplay.LastCard = null;
            }
            else
            {
                graveForDisplay.GraveCount = grave.ListOfCardsInGrave.Count;
                var lastCard = grave.ListOfCardsInGrave.LastOrDefault();
                if (lastCard == null)
                {
                    throw new Exception("ERROR");
                }
                graveForDisplay.LastCard= await this._cardService.MapCard(lastCard);
			}
            return graveForDisplay;
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
            cardsOnField=cardsOnField.OrderBy(x => x.FieldID).ToList();
            field.CardFields = cardsOnField;
            return field;
        }
		public EnemiesFieldDTO GetEneiesField(FieldDTO enemiesField)
		{
			var field = new EnemiesFieldDTO();
            field.LifePoints = enemiesField.LifePoints;
		    field.DeckCount= enemiesField.DeckCount;
            field.CardFields= enemiesField.CardFields;
            field.Hand = enemiesField.Hand;
			return field;
		}



        public void CardToBePlayedCheck(Game game,Player player,int cardInDeckID)
        {
			if (game.Turns.Count == 0)
			{
				throw new Exception("This game has no turns");
			}
			if (game.Turns.Last().MainPhase == false)
			{
				throw new Exception("You can play card only in main phase");
			}
			if (game.PlayerOnTurn != player.ID)
			{
				throw new Exception("It is not your turn to play");
			}
			if (player.Hand.CardsInHand?.Where(x => x.ID == cardInDeckID).FirstOrDefault() == null)
			{
				throw new Exception("Card with this ID is not in your hand");
			}
		}
		public async Task<FieldDTO> NormalSummon(int gameID,int playerID,int cardInDeckID,bool position)
        {
            var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			var turn = game.Turns?.Last();
			var player = await this._unitOfWork.Player.GetPlayerWithFieldsAndHand(playerID);
            this.CardToBePlayedCheck(game, player,cardInDeckID);
			if (turn.MonsterSummoned == true)
			{
				throw new Exception("You already summon monster in this turn");
			}
			var cardToBePlayed = await this._unitOfWork.CardInDeck.GetCardInDeckWithCard(cardInDeckID);
            if (cardToBePlayed.Card.CardType.Type != "MonsterCard")
            {
				throw new Exception("You can only summon monster card");
			}
			var emptyField=player.Fields.Where(x => x.CardOnField == null && x.FieldType == "MonsterField").FirstOrDefault();
            if(emptyField == null)
            {
				throw new Exception("Your field is full");
			}
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
            player.Hand.CardsInHand.Remove(player.Hand.CardsInHand?.Where(x=>x.ID==cardInDeckID).FirstOrDefault());
            turn.MonsterSummoned = true;
			this._unitOfWork.Turn.Update(turn);
			this._unitOfWork.Player.Update(player);
            await this._unitOfWork.Complete();
            var playerField = await this.GetPlayersField(playerID);
            return playerField;
		}

        public int NumberOfCardsRequiredForTribute(int numberOfStars)
        {
            if (numberOfStars < 0)
            {
                throw new Exception("There is some error");
            }
            if (numberOfStars <= 4)
            {
                return 0;//ne treba zrtva
            }
            else if (numberOfStars > 4 && numberOfStars <= 6)
            {
                return 1;//jedna zrtva
            }
            else if (numberOfStars > 6 && numberOfStars <= 8)
            {
                return 2;
            }
            else if(numberOfStars > 8)
            {
                return 3;
            }
            throw new Exception("There is some error");

        } 
        public async Task<FieldDTO> TributeSummon(List<int> fieldsIDs,int gameID, int playerID, int cardInDeckID,int numberOfStars, bool position)
        {

            var numberOfTributes = this.NumberOfCardsRequiredForTribute(numberOfStars);
            var game = await this._unitOfWork.Game.GetOne(gameID);
			if (game == null)
			{
				throw new Exception("There is no game with this id");
			}
			var grave = await this._unitOfWork.Grave.GetGrave((int)game.GraveID);
     
			if (numberOfTributes == 0)
            {
                return await this.NormalSummon(gameID,playerID,cardInDeckID,position);
            }
            if (numberOfTributes != fieldsIDs.Count())
            {
                throw new Exception("You didnt chose right number of tributes");
            }
            foreach (var fieldID in fieldsIDs)
            {
                var field = await this._unitOfWork.CardField.GetCardField(fieldID, playerID);
				if (field.FieldType != "MonsterField")
				{
					throw new Exception("This is no monster field");
				}
				if (field.CardOnField == null)
                {
					throw new Exception("There is no card on this field");
				}
                grave.ListOfCardsInGrave.Add(field.CardOnField);
                field.CardOnField=null;
                this._unitOfWork.CardField.Update(field);
			}
            this._unitOfWork.Grave.Update(grave);
            await this._unitOfWork.Complete();
			var playerField = await this.GetPlayersField(playerID);
			return playerField;
		}
        public async Task<AffterPlaySpellTrapCardData> PlaySpellCard(int gameID, int playerID, int cardInDeckID,int cardEffectID)
        {
            var affterPlayData = new AffterPlaySpellTrapCardData();
			var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
			var turn = game.Turns?.Last();
			var player = await this._unitOfWork.Player.GetPlayerWithFieldsAndHand(playerID);
            var effect = await this._unitOfWork.Effect.GetEffect(cardEffectID);
            this.CardToBePlayedCheck(game, player, cardInDeckID);
			var cardToBePlayed = await this._unitOfWork.CardInDeck.GetCardInDeckWithCard(cardInDeckID);
			if (cardToBePlayed.Card.CardType.Type != "SpellCard")
			{
				throw new Exception("You can only play spell card with this function");
			}
			var emptyField = player.Fields.Where(x => x.CardOnField == null && x.FieldType == "SpellTrapField").FirstOrDefault();
			if (emptyField == null)
			{
				throw new Exception("Your field is full");
			}
			emptyField.CardOnField = cardToBePlayed;

			player.Hand.CardsInHand.Remove(player.Hand.CardsInHand.Where(x => x.ID == cardInDeckID).FirstOrDefault());
			this._unitOfWork.Player.Update(player);
			await this._unitOfWork.Complete();

			var concreteStrategy = this._concreteStrategy.SetStrategyContext(effect.EffectType.Type);
			int areaOfClicking = concreteStrategy.GetAreaOfSelectingCards();

            affterPlayData.areaOfClicking = areaOfClicking;
            affterPlayData.fieldID = emptyField.ID;
            return affterPlayData;
		}
		public async Task ExecuteEffect(List<int> listOfCards, int cardFieldID,int playerID,int gameID)
        {
            var field = await this._unitOfWork.CardField.GetCardField(cardFieldID,playerID);
            if (field.CardOnField == null)
            {
                throw new Exception("This card has no effect");
            }
            var effect = await this._unitOfWork.Effect.GetEffect((int)field.CardOnField.Card.EffectID);
            if (listOfCards.First() != 0)
            {
                if (effect.NumOfCardsAffected != listOfCards.Count)
                {
                    throw new Exception("You didint chose right amount of cards");
                }
            }
			var concreteStrategy = this._concreteStrategy.SetStrategyContext(effect.EffectType.Type);
			await concreteStrategy.ExecuteEffect(listOfCards,effect,playerID,gameID, cardFieldID);
            await this._unitOfWork.Complete();

		}
        public async Task RemoveCardFromFieldToGrave(int fieldID, int gameID,int playerID)
        {
            var cardField = await this._unitOfWork.CardField.GetCardField(fieldID,playerID);
            if (cardField.CardOnField == null)
            {
                throw new Exception("There is no card on this field");
            }
            var grave = await this._unitOfWork.Grave.GetGraveByGameID(gameID);
            grave.ListOfCardsInGrave.Add(cardField.CardOnField);
            cardField.CardOnField =null;
            this._unitOfWork.Grave.Update(grave);
			this._unitOfWork.CardField.Update(cardField);
            await this._unitOfWork.Complete();
		}
		public async Task RemoveCardFromHandToGrave(int playerID,int cardID, int gameID)
		{
			var playerHnad = await this._unitOfWork.Player.GetPlayersHand(playerID);
            var card = playerHnad.CardsInHand?.Where(x => x.ID == cardID).FirstOrDefault();
            if (card == null)
            {
                throw new Exception("This card is not in your hand");
            }
			var grave = await this._unitOfWork.Grave.GetGraveByGameID(gameID);
			grave.ListOfCardsInGrave.Add(card);
            playerHnad.CardsInHand.Remove(card);
			this._unitOfWork.Grave.Update(grave);
			this._unitOfWork.PlayerHand.Update(playerHnad);
			await this._unitOfWork.Complete();
		}
        public async Task ChangeMonsterPosition(int playerID,int fieldID,int gameID)
        {
            var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
            if (game.PlayerOnTurn != playerID)
            {
				throw new Exception("You are not on turn");
			}
			var curentTurn = game.Turns?.LastOrDefault();
            if (curentTurn == null)
            {
				throw new Exception("This game has no turns");
			}
            if (curentTurn.MainPhase == false) 
            { 
				throw new Exception("You can change card position only in main phase");
			}
            var cardField=await this._unitOfWork.CardField.GetCardField(fieldID,playerID);
            if (cardField.PlayerID != playerID)
            {
				throw new Exception("This is not your field");
			}
            if (cardField.CardOnField == null)
            {
				throw new Exception("There is no card on this field");
			}
            cardField.CardPosition=!cardField.CardPosition;
            if (cardField.CardShowen == false)
            {
                cardField.CardShowen = true;
            }
            this._unitOfWork.CardField.Update(cardField);
            await this._unitOfWork.Complete();
		}
        public async Task<int> AttackEnemiesField(int attackingFieldID, int attackedFieldID, int playerID, int enemiesPlayerID, int gameID)
        {
            var game = await this._unitOfWork.Game.GetGameWithTurns(gameID);
            if (game.PlayerOnTurn != playerID)
            {
                throw new Exception("You are not on turn");
            }
            var curentTurn = game.Turns?.LastOrDefault();
            if (curentTurn == null)
            {
                throw new Exception("This game has no turns");
            }
            if (curentTurn.BattlePhase == false)
            {
                throw new Exception("You can attack only in battle phase");
            }
			var attackingField = await this._unitOfWork.CardField.GetCardField(attackingFieldID, playerID);
			if (attackingField.CardOnField == null)
			{
				throw new Exception("You cant attack from empty field");
			}
			if (attackingField.CardPosition == false)
			{
				throw new Exception("Your card is in deffence position");
			}
			if (attackingField.FieldType == "MonsterType")
			{
				throw new Exception("You can only attack from MonterField");
			}
			var posibleAttack = await this._unitOfWork.AttackInTurn.GetAttackInTurnByTurnIDAndFieldID(attackingFieldID, curentTurn.ID);
			var attackedField = await this._unitOfWork.CardField.GetCardField(attackedFieldID, enemiesPlayerID);
			if (attackedField.FieldType=="MonsterField" && attackedField.CardOnField == null)
			{
				throw new Exception("You cant attack empty field");
			}
			var concreteStrategy = this._concreteAttackingStrategy.SetStrategyContext(attackedField.CardPosition,attackedField.FieldType);
			var difference=await concreteStrategy.AttackEnemie(attackingField,attackedField,gameID);
			posibleAttack.IsAttackingAbble = false;
			this._unitOfWork.AttackInTurn.Update(posibleAttack);
            await this._unitOfWork.Complete();
            return difference;
		}


	}
}
