import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { BehaviorSubject, elementAt, Subscription } from 'rxjs';
import { OnlineUsers } from 'src/classes/user';
import { AuthService } from '../../services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { ConnectionService } from 'src/app/services/connection.service';
import { OnlineusersService } from 'src/app/services/onlineusers.service';
import { GamesRequestsService } from 'src/app/services/games-requests.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  hubConnection!: signalR.HubConnection;
  public message: string = '';
  public messages: string[] = [];
  usersOnline:Array<OnlineUsers>=new Array<OnlineUsers>();;
  subscripions: Subscription[] = []
  userID=this.authService?.userValue?.id
  gameRequests!:any[];
  game:any;

  constructor( 
    private authService: AuthService,
    private signalrService:SignalrService,
    private connectionService:ConnectionService,
    private onlineUsersService:OnlineusersService,
    private gr:GamesRequestsService,
    private router: Router

    ) { }
  
  ngOnInit(): void {
    this.connectionService.getConnectionID();
    this.onlineUsersService.getOnlineUsersList();
    this.getOnlineUsers();
    this.getGameRequestsList();
    
    if(this.userID!=undefined){
      if (this.signalrService.hubConnection.state=="Connected") {
        this.connectionService.getConnectionIDInv();
        this.addConncectionInv(this.userID);
        this.onlineUsersService.getOnlineUsersInv();
        // this.getOnlineUsersInv();
        
        this.getGameRequestsInv(this.userID);
      }
      else{
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == "HubConnStarted") {
            this.connectionService.getConnectionIDInv();
            this.addConncectionInv(this.userID);
            this.onlineUsersService.getOnlineUsersInv();
            // this.getOnlineUsersInv();
            this.getGameRequestsInv(this.userID);
          }
        });
      }
    }
  }

  // getOnlineUsersInv(): void 
  // {
  //   this.signalrService.hubConnection.invoke("GetOnlineUsers")
  //   .catch(err => console.error(err));
  // }

  private getOnlineUsers(): void
  {
    this.subscripions.push(
      this.onlineUsersService.usersOnline.subscribe(res=>{
        this.usersOnline=res.filter(x=>x.id!=this.userID); 
      }))
  }

  getGameRequestsInv(user1:number): void {
    this.signalrService.hubConnection.invoke("GamesRequests", user1)
    .catch(err => console.error(err));
  }

  getGameRequestsList()
  {
    this.signalrService.hubConnection.on("GetGamesRequests", (users:any[]) => {
      this.gameRequests=users;
      console.log("gameRequests",this.gameRequests);
    });
  }

  addUserInv(user1:number,user2:number): void {
    console.log("moja braca su u lovi",user1,user2)
    this.signalrService.hubConnection.invoke("CreateLobby", user1, user2)
    .catch(err => console.error(err));

  }
  addUsersToLobby(user1:any,user2:any)
  {
    // this.signalrService.hubConnection.on("AddUsersToLobby",(user1,user2))
  }

  addConncectionInv(userID:any): void {
    this.signalrService.hubConnection.invoke("AddConnection",userID)
    .catch(err => console.error(err));
  }
  startGameInv(gameID:any)
  {
    this.signalrService.hubConnection.invoke("StartGame",gameID)
    .catch(err => console.error(err));
  }
  startGameResponse()
  {
    this.signalrService.hubConnection.on("GameStarted", (game:any) => {
      console.log("bla")
      this.router.navigate(['/game']);
    });
  }

  challangePlayer(user1:any,user2:any)
  {
    if (this.signalrService.hubConnection.state=="Connected") {
        this.addUserInv(user1,user2);
        this.getGameRequestsInv(user2);
        this.getGameRequestsList();
    }
  }

  acceptGameRequest(lobbyID:any)
  {
    console.log("accepted", lobbyID)
    this.gr.startGame(lobbyID).subscribe({
      next: res=> {
        console.log("uspesno prihvacen zahtev")
        this.startGameResponse();
        this.startGameInv(res);
        
      },
      error: err=> {
        console.log("neuspesno")
      }
    });
  }

  denyGameRequest(lobbyID:any)
  {
    console.log("dennied", lobbyID)
    this.gr.denyGame(lobbyID).subscribe({
      next: res=> {
        console.log("uspesno odbijen zahtev")
        this.getGameRequestsInv(this.userID);
        this.getGameRequestsList();
      },
      error: err=> {
        console.log("neuspesno odbijen zahtev")
      }
    })
  }
  
  ngOnDestroy(): void {
    this.subscripions.forEach(sub=>sub.unsubscribe())
  }
}
