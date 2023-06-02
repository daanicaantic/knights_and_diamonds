import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SignalrService } from './signalr.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  constructor(
    private httpClient: HttpClient,
    private signalrService: SignalrService,
    private router: Router
  ) {}

  getTurnInfoInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('GetStartingTurnInfo', Number(gameID), playerID)
      .catch((err) => console.error(err));
  }

  gatFieldInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('GetPlayersField', gameID, playerID)
      .catch((err) => console.error(err));
  }

  startingDrawingInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('StartingDrawing', gameID, playerID)
      .catch((err) => console.error(err));
  }
  drawPhaseInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('DrawPhase', Number(gameID), playerID)
      .catch((err) => console.log(err));
  }
  battlePhaseInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('BattlePhase', Number(gameID), playerID)
      .catch((err) => console.log(err));
  }
  getTurnPhaseInv(gameID: any): void {
    this.signalrService.hubConnection
      .invoke('GetTurnPhase', gameID)
      .catch((err) => console.error('O V D E', err));
  }
  attackEnemiesFieldInv(
    gameID: any,
    playerID: any,
    fieldID: any,
    attackedField: any
  ) {
    this.signalrService.hubConnection
      .invoke('AttackEnemiesField', gameID, playerID, fieldID, attackedField)
      .catch((err) => console.error('O V D E', err));
  }
  executeEffectInv(
    listOfCards: any,
    cardFieldID: any,
    playerID: any,
    gameID: any
  ) {
    console.log('EXECUTEEFFECT', listOfCards, cardFieldID, playerID, gameID);
    this.signalrService.hubConnection
      .invoke(
        'ExecuteEffect',
        listOfCards,
        cardFieldID,
        playerID,
        Number(gameID)
      )
      .catch((err) => console.error(err));
  }
  normalSummonInv(
    gameID: any,
    playerID: any,
    cardID: any,
    position: any
  ): void {
    this.signalrService.hubConnection
      .invoke('NormalSummon', gameID, playerID, cardID, position)
      .catch((err) => console.log('O V D E', err.message));
  }
  playSpellCardInv(
    gameID: any,
    playerID: any,
    cardID: any,
    cardEffectID: any
  ): void {
    console.log(gameID, playerID, cardID, cardEffectID);
    this.signalrService.hubConnection
      .invoke('PlaySpellCard', gameID, playerID, cardID, cardEffectID)
      .catch((err) => console.error('O V D E', err));
  }

  getGame(gameID: any, userID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetGame/` + `${gameID}` + `/` + `${userID}`
    );
  }

  getHand(gameID: any, playerID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetHand/` + `${gameID}` + `/` + `${playerID}`
    );
  }
  getPlayersField(playerID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetField/` + `${playerID}`
    );
  }
  // getEnemiesField(playerID: any) {
  //   return this.httpClient.get(
  //     `https://localhost:7250/Game/GetEnemiesField/` + `${playerID}`
  //   );
  // }
  // getPlayerOnTurn(gameID: any) {
  //   return this.httpClient.get(
  //     `https://localhost:7250/Game/GetPlayerOnTurn/` + `${gameID}`
  //   );
  // }

  getStartingField(playerID: any, enemiesPlayerID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetStartingField/` +
        `${playerID}/` +
        `${enemiesPlayerID}`
    );
  }

  getTurnInfo(gameID: any, playerID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetTurnInfo/` +
        `${gameID}` +
        `/` +
        `${playerID}`
    );
  }
  getGrave(gameID: any) {
    return this.httpClient.get(
      `https://localhost:7250/Game/GetGrave/` + `${gameID}`
    );
  }
  newTurn(gameID: any) {
    return this.httpClient.post(
      `https://localhost:7250/Game/NewTurn/` + `${gameID}`,
      {}
    );
  }
}
