import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { Connection } from 'src/classes/connection';

@Injectable({
  providedIn: 'root'
})
export class OnelineusersService {
  connections:Array<Connection>=new Array<Connection>();
  connectionsObs:BehaviorSubject<any[]> = new BehaviorSubject<any[]>([])

  constructor() { }

  hubConnection!: signalR.HubConnection;

  startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7250/toastr', {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
      })
      .build();
      console.log("p o r u k e");
          
          //this lines up with the method called by `SendAsync`
          this.hubConnection.on("GetUsersFromHub", (c: Array<any[]>) => {
            console.log('c',c);
              this.connectionsObs.next(c)
            });
            
          console.log("ovdeeeeeeeeeeeeeee")
          //this will start the long polling connection
          this.hubConnection.start()
              .then(() => { console.log("Connection started"); 
              this.hubConnection.invoke("getOnlineUsers")
              .catch(err => console.error(err));
              })
              .catch(err => { console.error(err); });
      }
  }

