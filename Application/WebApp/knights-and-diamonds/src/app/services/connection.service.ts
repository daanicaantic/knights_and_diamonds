import { Injectable } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr.service';


@Injectable({
  providedIn: 'root'
})
export class ConnectionService {
  connectionID:any;
    
  constructor(private signalrService:SignalrService,) { }

  getConnectionIDInv(): void {
    this.signalrService.hubConnection.invoke("GetConnection")
    .catch(err => console.error(err));
  }

  getConnectionID()
  {
    this.signalrService.hubConnection.on("GetConnectionID", (connecionID:any) => {
      console.log("connectionID",connecionID)
      this.connectionID=connecionID;
    });
  }
}
