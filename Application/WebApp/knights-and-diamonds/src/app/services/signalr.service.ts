import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
  
})
export class SignalrService {

  constructor(private messageService: MessageService,) { }

  hubConnection!: signalR.HubConnection;

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
          console.log('Hub Connection Started!');
          this.askServerListener();
          this.askServer();
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }
  async askServer() {
    console.log("askServerStart");

    await this.hubConnection.invoke("askServer", "hi")
        .then(() => {
            console.log("askServer.then");
        })
        .catch(err => console.error(err));

    console.log("This is the final prompt");
}

askServerListener() {
    console.log("askServerListenerStart");

    this.hubConnection.on("askServerResponse", (someText) => {
        console.log("askServer.listener");
        this.messageService.add({key: 'br', severity:'success', summary: 'Uspešno', detail: someText});
    })
}
}
