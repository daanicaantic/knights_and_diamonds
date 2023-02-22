import { Injectable } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {
  connectionID: any
    
  constructor(private signalrService: SignalrService) { }

  addConnectionInv(userID:any): void {
    this.signalrService.hubConnection.invoke("AddConnection", userID)
    .catch(err => console.error(err));
  }
}
