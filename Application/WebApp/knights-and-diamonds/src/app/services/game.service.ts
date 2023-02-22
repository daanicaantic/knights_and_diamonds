import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { SignalrService } from './signalr.service';
import { Router } from '@angular/router';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private httpClient: HttpClient,
    private signalrService: SignalrService,
    private router: Router
  ) { }

  startGame(lobbyID: any) {
    return this.httpClient.post(`https://localhost:7250/RPSGame/StartGame/` + `${lobbyID}`, { responseType: 'text' });
  }

  denyGame(lobbyID: any) {
    console.log(lobbyID)
    return this.httpClient.delete(`https://localhost:7250/RPSGame/DenyGame/` + `${lobbyID}`);
  }

  startGameInv(gameID: any) {
    this.signalrService.hubConnection.invoke("StartGame", gameID)
      .catch(err => console.error(err));
  }

  startGameResponse() {
    this.signalrService.hubConnection.on("GameStarted", (gameID: any) => {
      this.router.navigate(['/game', gameID]);
    });
  }

  getPlayer(gameID: any, userID: any) {
    return this.httpClient.get(`https://localhost:7250/RPSGame/GetPlayer/` + `${gameID}` + `/` + `${userID}`);
  }

  removeUserFromUsersInGame(userID: any) {
    return this.httpClient.delete(`https://localhost:7250/RPSGame/RemoveUserFromUsersInGame/` + `${userID}`);
  }

  playMove(playerID: any, moveName: string) {
    return this.httpClient.put(`https://localhost:7250/RPSGame/PlayMove/` + `${playerID}` + `/` + `${moveName}`, { responseType: 'text' });
  }
}
