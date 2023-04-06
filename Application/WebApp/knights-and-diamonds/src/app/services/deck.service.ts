import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeckService {

  constructor(private httpClient: HttpClient) { }

  getUsersDeck(userID: any) {
    return this.httpClient.get(`https://localhost:7250/Deck/GetDeck/` + `${userID}`);
  }
}
