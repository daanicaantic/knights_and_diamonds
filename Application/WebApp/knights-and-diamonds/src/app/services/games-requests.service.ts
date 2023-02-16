import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root'
})
export class GamesRequestsService {

  constructor(private httpClient:HttpClient) { }

  startGame(lobbyID:any) {
    return this.httpClient.post(`https://localhost:7250/RPSGame/StartGame/`+`${lobbyID}`,{responseType:'text'});
  }
  
  denyGame(lobbyID:any) {
    console.log(lobbyID)
    return this.httpClient.delete(`https://localhost:7250/RPSGame/DenyGame/`+`${lobbyID}`);
  }
}
