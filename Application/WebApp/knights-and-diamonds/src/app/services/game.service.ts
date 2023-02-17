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
  constructor(private httpClient:HttpClient,
    private signalrService:SignalrService,
    private router: Router

    ) { }
  
  startGame(lobbyID:any) {
    return this.httpClient.post(`https://localhost:7250/RPSGame/StartGame/`+`${lobbyID}`,{responseType:'text'});
  }
  
  denyGame(lobbyID:any) {
    console.log(lobbyID)
    return this.httpClient.delete(`https://localhost:7250/RPSGame/DenyGame/`+`${lobbyID}`);
  }

  startGameInv(gameID:any) {
    this.signalrService.hubConnection.invoke("StartGame",gameID)
    .catch(err => console.error(err));
  }
  startGameResponse() {
    this.signalrService.hubConnection.on("GameStarted", (game:any) => {
      this.router.navigate(['/game']);
    });
  }
}
