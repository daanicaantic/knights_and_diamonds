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

  getGame(gameID: any, userID: any) {
    return this.httpClient.get(`https://localhost:7250/Game/GetGame/` + `${gameID}` + `/` + `${userID}`);
  }
}
