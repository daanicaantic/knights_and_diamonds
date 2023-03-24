import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IngameService {

  gameStarted = new Subject<any>();
  gameObs(): Observable<any> {
    return this.gameStarted.asObservable();
  }
  constructor() { }

  setGameOn(): void {
    this.gameStarted.next({ type: "GameOn" })
  }
  setGameOff():void{
    this.gameStarted.next({ type: "GameOff" })
  }
}
