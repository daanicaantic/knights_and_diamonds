import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { MessageService } from 'primeng/api';
import { Observable, Subject } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
  
})
export class SignalrService {

  constructor(private messageService: MessageService
  ) { }

  hubConnection!: signalR.HubConnection;
  
    
  ssSubj = new Subject<any>();
  ssObs(): Observable<any> {
    return this.ssSubj.asObservable();
  }

  startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7250/toastr', {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
      })
      .build();

      this.hubConnection
      .start()
      .then(() => {
        this.ssSubj.next({type:"HubConnStarted"})
        console.log("dfhasdkjlfbjsdafsajdj",this.hubConnection.connectionId)
        
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

//   async askServer() {
//     console.log("askServerStart");

//     await this.hubConnection.invoke("askServer", "hi")
//         .then(() => {
//             console.log("askServer.then");
//         })
//         .catch(err => console.error(err));

//     console.log("This is the final prompt");
// }

// askServerListener() {
//     console.log("askServerListenerStart");

//     this.hubConnection.on("askServerResponse", (someText) => {
//         console.log("askServer.listener");
//         this.messageService.add({key: 'br', severity:'success', summary: 'Uspešno', detail: someText});
//     })
// }
}
