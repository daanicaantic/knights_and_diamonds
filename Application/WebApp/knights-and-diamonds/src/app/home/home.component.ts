import { Component, OnInit } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { HubConnection } from '@aspnet/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  hubConnection!: signalR.HubConnection;
  public message: string = '';
  public messages: string[] = [];
  constructor() { }

  ngOnInit(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7250/toastr', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
        
    })
    .build();
    console.log("ovde")
        
        //this lines up with the method called by `SendAsync`
        this.hubConnection.on("Send", (msg) => {
            this.messages.push(msg);
        });
        
        //this will start the long polling connection
        this.hubConnection.start()
            .then(() => { console.log("Connection started"); })
            .catch(err => { console.error(err); });
    }

    echo() {
        //this will call the method in the EchoHub
        this.hubConnection.invoke("Echo", this.message);
    }

}
