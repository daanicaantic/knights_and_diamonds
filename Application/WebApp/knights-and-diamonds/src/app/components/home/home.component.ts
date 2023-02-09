// import { Component, OnDestroy, OnInit } from '@angular/core';
// import * as signalR from '@aspnet/signalr';
// import { HubConnection } from '@aspnet/signalr';
// import { BehaviorSubject, Subscription } from 'rxjs';
// import { Connection } from 'src/classes/connection';
// import { OnlineUsers } from 'src/classes/user';
// import { AuthService } from '../../services/auth.service';
// import { OnelineusersService } from '../../services/onelineusers.service';

// @Component({
//   selector: 'app-home',
//   templateUrl: './home.component.html',
//   styleUrls: ['./home.component.css']
// })
// export class HomeComponent implements OnInit, OnDestroy {
//   hubConnection!: signalR.HubConnection;
//   public message: string = '';
//   public messages: string[] = [];
//   usersOnline:Array<OnlineUsers>=new Array<OnlineUsers>();
//   subscripions: Subscription[] = []
//   userID=this.authService?.userValue?.id
//   empty:any;

//   constructor( public onelineusers:OnelineusersService, 
//     private authService: AuthService,) { }

//   ngOnInit(): void {
//     console.log("usero",this.userID)
//     if(this.userID!=undefined){
//     this.onelineusers.startConnection();
//     this.subscripions.push(
//       this.onelineusers.connectionsObs.subscribe(res=>{
//         res=res.filter(u=>u.id!==this.userID);
//         this.usersOnline=res; 
//       }
//       ))
//     }
  
//     }

//     ngOnDestroy(): void {
//       this.subscripions.forEach(sub=>sub.unsubscribe())
//     }
// }

import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { HubConnection } from '@aspnet/signalr';
import { BehaviorSubject, elementAt, Subscription } from 'rxjs';
import { Connection } from 'src/classes/connection';
import { OnlineUsers } from 'src/classes/user';
import { AuthService } from '../../services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { Dictionary } from 'src/classes/dictionary';
import { ConnectionService } from 'src/app/services/connection.service';

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
  dictionary!:Array<Dictionary>;
  diction!:Dictionary;
  subscripions: Subscription[] = []
  userID=this.authService?.userValue?.id
  empty:any;
  users:any[]=[];
  pomocna:boolean=false;

  constructor( 
    private authService: AuthService,
    private signalrService:SignalrService,
    private connectionService:ConnectionService,
    ) { }
  
  ngOnInit(): void {
    console.log("idusera",this.userID)
    this.getOnlineUsersList();
    this.getGameRequestsList();
    this.connectionService.getConnectionID();
    
    if(this.userID!=undefined){
      if (this.signalrService.hubConnection.state=="Connected") {
        this.getOnlineUsersInv();
        this.getGameRequestsInv(this.userID);
        this.connectionService.getConnectionIDInv();
      }
      else{
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == "HubConnStarted") {
            this.getOnlineUsersInv();
            this.getGameRequestsInv(this.userID);
            this.connectionService.getConnectionIDInv();
          }
        });
      }
    }
  }

  getOnlineUsersInv(): void {
    this.signalrService.hubConnection.invoke("GetOnlineUsers")
    .catch(err => console.error(err));
  }

  addUserInv(user1:number,user2:number): void {
    console.log("moja braca su u lovi",user1,user2)
    this.signalrService.hubConnection.invoke("CreateLobby",user1,user2)
    .catch(err => console.error(err));

  }

  getGameRequestsInv(user1:number): void {
    console.log("moja braca su u lovi",user1)
    this.signalrService.hubConnection.invoke("GamesRequests",user1)
    .catch(err => console.error(err));
  }

  getGameRequestsList()
  {
    this.signalrService.hubConnection.on("GetGamesRequests", (users:any[]) => {
      console.log("ovdeeeeeeee",this.userID,users)
    });
  }

  addUsersToLobby(user1:any,user2:any)
  {
    // this.signalrService.hubConnection.on("AddUsersToLobby",(user1,user2))
  }
  private getOnlineUsersList(): void
  {
    this.signalrService.hubConnection.on("GetUsersFromHub", (users: Array<OnlineUsers>) => {
      this.usersOnline=[...users];
      if(this.userID!==undefined){
        this.usersOnline=this.usersOnline.filter(x=>x.id != this.userID)
      }
      });
  }

  ChallangePlayer(user1:any,user2:any)
  {
    console.log(user1,user2);
    if (this.signalrService.hubConnection.state=="Connected") {
        this.addUserInv(user1,user2);

        this.getGameRequestsInv(this.userID);
        this.getGameRequestsList();
        console.log("blabal")
    }
  }
  
  ngOnDestroy(): void {
    this.subscripions.forEach(sub=>sub.unsubscribe())
  }
}
