import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SignalrService } from './signalr.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  constructor(
    private httpClient: HttpClient,
    private signalrService: SignalrService,
    private router: Router,
    private messageService: MessageService
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
  endPhaseInv(gameID: any, playerID: any, enemiesID: any): void {
    this.signalrService.hubConnection
      .invoke('EndPhase', Number(gameID), playerID, enemiesID)
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
    attackedField: any,
    enemiesID: any
  ) {
    this.signalrService.hubConnection
      .invoke(
        'AttackEnemiesField',
        fieldID,
        attackedField,
        playerID,
        enemiesID,
        gameID
      )
      .catch((err) => console.error('O V D E', err));
  }
  executeEffectInv(
    listOfCards: any,
    cardFieldID: any,
    playerID: any,
    enemiesID: any,
    gameID: any
  ) {
    console.log(
      'EXECUTEEFFECT',
      listOfCards,
      cardFieldID,
      playerID,
      enemiesID,
      gameID
    );
    this.signalrService.hubConnection
      .invoke(
        'ExecuteEffect',
        listOfCards,
        cardFieldID,
        playerID,
        enemiesID,
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
      .catch((err) =>
        this.messageService.add({
          key: 'br',
          severity: 'error',
          summary: 'Neuspešno',
          detail: err.toString(),
        })
      );
  }
  playSpellTrapCardInv(
    gameID: any,
    playerID: any,
    cardID: any,
    cardEffectID: any,
    isSpellOrTrap: any
  ): void {
    console.log(gameID, playerID, cardID, cardEffectID);
    this.signalrService.hubConnection
      .invoke(
        'PlaySpellTrapCard',
        gameID,
        playerID,
        cardID,
        cardEffectID,
        isSpellOrTrap
      )
      .catch((err) => console.error('O V D E', err));
  }
  activateTrapCard(gameID: any, playerID: any, cardFieldID: any): void {
    console.log(gameID, playerID, cardFieldID);
    this.signalrService.hubConnection
      .invoke('ActiveTrapCard', Number(gameID), playerID, cardFieldID)
      .catch((err) => console.error('O V D E', err));
  }
  removeCardFromHandToGraveInv(playerID: any, cardID: any, gameID: any): void {
    this.signalrService.hubConnection
      .invoke('RemoveCardFromHandToGrave', playerID, cardID, Number(gameID))
      .catch((err) => console.error('O V D E', err));
  }
  removeCardFromFieldToGraveInv(
    fieldID: any,
    playerID: any,
    gameID: any
  ): void {
    this.signalrService.hubConnection
      .invoke('RemoveCardFromFieldToGrave', fieldID, playerID, Number(gameID))
      .catch((err) => console.error('O V D E', err));
  }
  didTrapEffectExecutedInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('DidTrapEffectExecuted', Number(gameID), playerID)
      .catch((err) => console.error('O V D E', err));
  }
  getWinnerInv(gameID: any, playerID: any): void {
    this.signalrService.hubConnection
      .invoke('GetWinner', Number(gameID), playerID)
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
