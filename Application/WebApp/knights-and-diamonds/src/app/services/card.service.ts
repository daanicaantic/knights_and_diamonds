import {
  HttpClient,
  HttpErrorResponse,
  HttpEventType,
  HttpHeaders,
} from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root',
})
export class CardService {
  progress!: number;
  message!: string;
  constructor(private httpClient: HttpClient) {}

  addCard(card: any): any {
    return this.httpClient.post(`https://localhost:7250/Card/AddCard`, card);
  }
  updateCard(card: any): any {
    return this.httpClient.put(`https://localhost:7250/Card/UpdateCard`, card);
  }
  deleteCard(cardID: any):any {
    return this.httpClient.delete(`https://localhost:7250/Card/DeleteCard/`+ `${cardID}`);
  }

  getCards() {
    return this.httpClient.get(`https://localhost:7250/Card/GetAllCards`);
  }

  getCardTypes() {
    return this.httpClient.get(`https://localhost:7250/Types/GetCardTypes`);
  }

  getEffectTypes() {
    return this.httpClient.get(`https://localhost:7250/Types/GetEffectTypes`);
  }
  getFilteredCards(
    typeFilter: any,
    sortOrder: any,
    nameFilter: any,
    pageNumber: any,
    pageSize: any
    ) {
    return this.httpClient.get(
      `https://localhost:7250/Card/GetFillteredCards?typeFilter=${typeFilter}&sortOrder=${sortOrder}&nameFilter=${nameFilter}&pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }
  cardCount() {
    return this.httpClient.get(`https://localhost:7250/Card/CardCount`);
  }
}
