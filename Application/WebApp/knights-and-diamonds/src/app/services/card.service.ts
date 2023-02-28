import {HttpClient,HttpErrorResponse,HttpEventType,HttpHeaders } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root'
})
export class CardService {
  progress!: number;
  message!: string;
  constructor(private httpClient: HttpClient) { }

  addCard(card: any): any {
    return this.httpClient.post(`https://localhost:7250/Card/AddCard`, card);
  }
  addSTCard(card: any): any {
    return this.httpClient.post(`https://localhost:7250/Card/AddSTCard`, card);
  }

  getCardTypes() {
    return this.httpClient.get(`https://localhost:7250/Types/GetCardTypes`);
  }

  getMonsterTypes() {
    return this.httpClient.get(`https://localhost:7250/Types/GetMonsterTypes`);
  }

  getElementTypes() {
    return this.httpClient.get(`https://localhost:7250/Types/GetElementTypes`);
  }

  
}
