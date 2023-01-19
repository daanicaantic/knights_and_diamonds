// import { Component, OnInit } from '@angular/core';
// import * as signalR from '@aspnet/signalr';
// import { HubConnection } from '@aspnet/signalr';

// @Component({
//   selector: 'app-home',
//   templateUrl: './home.component.html',
//   styleUrls: ['./home.component.css']
// })
// export class HomeComponent implements OnInit {
//   hubConnection!: signalR.HubConnection;
//   public message: string = '';
//   public messages: string[] = [];
//   constructor() { }

//   ngOnInit(): void {
//     this.hubConnection = new signalR.HubConnectionBuilder()
//     .withUrl('https://localhost:7250/toastr', {
//         skipNegotiation: true,
//         transport: signalR.HttpTransportType.WebSockets,    
//     })
//     .build();
//     console.log("p o r u k e",this.messages);
        
//         //this lines up with the method called by `SendAsync`
//         this.hubConnection.on("Send", (msg) => {
//             this.messages.push(msg);
//         });
//         console.log("ovdeeeeeeeeeeeeeee")
//         //this will start the long polling connection
//         this.hubConnection.start()
//             .then(() => { console.log("Connection started"); 
                
//             })
//             .catch(err => { console.error(err); });
//     }

//     echo() {
//         console.log(this.messages);
//         //this will call the method in the EchoHub
//         this.hubConnection.invoke("Echo", this.message);
//     }

// }
import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { HubConnection } from '@aspnet/signalr';
import { BehaviorSubject, Subscription } from 'rxjs';
import { Connection } from 'src/classes/connection';
import { OnlineUsers } from 'src/classes/user';
import { OnelineusersService } from '../services/onelineusers.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  hubConnection!: signalR.HubConnection;
  public message: string = '';
  public messages: string[] = [];
  usersOnline:Array<OnlineUsers>=new Array<OnlineUsers>();


  subscripions: Subscription[] = []
 
  constructor( public onelineusers:OnelineusersService ) { 
    
  }

  ngOnInit(): void {
    this.onelineusers.startConnection();
    this.subscripions.push(
      this.onelineusers.connectionsObs.subscribe(res=>{
        this.usersOnline=res; 
      }))
    }

    ngOnDestroy(): void {
      this.subscripions.forEach(sub=>sub.unsubscribe())
    }
}