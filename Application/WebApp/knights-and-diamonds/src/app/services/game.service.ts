import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SignalrService } from './signalr.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  constructor(
    private httpClient: HttpClient,
    private signalrService: SignalrService,
    private router: Router
  ) { }

  gatFieldInv(gameID:any,playerID:any): void {
    console.log(gameID,playerID);
    this.signalrService.hubConnection.invoke("GetPlayersField",gameID,playerID)
      .catch(err => console.error(err));
  }
  startingDrawingInv(gameID:any,playerID:any): void {
    this.signalrService.hubConnection.invoke("StartingDrawing",gameID,playerID)
      .catch(err => console.error(err));
  }

  getGame(gameID: any, userID: any) {
    return this.httpClient.get(`https://localhost:7250/Game/GetGame/` + `${gameID}` + `/` + `${userID}`);
  }

  getHand(gameID: any, playerID: any) {
    return this.httpClient.get(`https://localhost:7250/Game/GetHand/` + `${gameID}` + `/` + `${playerID}`);
  }
  getPlayersField(playerID: any) {
    return this.httpClient.get(`https://localhost:7250/Game/GetField/` + `${playerID}`);
  }
  getEnemiesField(playerID: any) {
    return this.httpClient.get(`https://localhost:7250/Game/GetEnemiesField/` + `${playerID}`);
  }
}
