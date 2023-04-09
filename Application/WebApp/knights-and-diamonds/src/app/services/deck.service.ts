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

  addCardToDeck(cardID: any, deckID: any) {
    return this.httpClient.post(`https://localhost:7250/Deck/AddCardToDeck/` + `${cardID}` + `/` + `${deckID}`, {});
  }

  removeCardFromDeck(cardID: any, deckID: any) {
    return this.httpClient.delete(`https://localhost:7250/Deck/RemoveCardFromDeck/` + `${cardID}` + `/` + `${deckID}`);
  }

  cardCounter(deckID: any, userID: any) {
    return this.httpClient.get(`https://localhost:7250/Deck/CardCounter/` + `${deckID}` + `/` + `${userID}`)
  }
}
