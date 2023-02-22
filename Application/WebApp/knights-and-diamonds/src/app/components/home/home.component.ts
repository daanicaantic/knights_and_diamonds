import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, elementAt, Observable, Subject, Subscription } from 'rxjs';
import { OnlineUsers } from 'src/classes/user';
import { AuthService } from '../../services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { OnlineusersService } from 'src/app/services/onlineusers.service';
import { GameService } from 'src/app/services/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  public message: string = '';
  public messages: string[] = [];
  usersOnline:Array<OnlineUsers>=new Array<OnlineUsers>();
  subscripions: Subscription[] = []
  userID=this.authService?.userValue?.id
  gameRequests!:any[];

  constructor( 
    private authService: AuthService,
    private signalrService:SignalrService,
    private onlineUsersService:OnlineusersService,
    private gameService:GameService,
    private router: Router
    ) {}
  
  ngOnInit(): void {
    this.onlineUsersService.getOnlineUsersList();
    this.getOnlineUsers();
    this.getGameRequestsList();
    this.gameService.startGameResponse();
 
    if(this.userID!=undefined){
      if (this.signalrService.hubConnection.state=="Connected") {
        this.onlineUsersService.getOnlineUsersInv();
        this.getGameRequestsInv(this.userID);
      }
      else{
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == "HubConnStarted") {
            this.onlineUsersService.getOnlineUsersInv();
            this.getGameRequestsInv(this.userID);
          }
        });
      }
    }
  }

  ngOnDestroy(): void {
    this.subscripions.forEach(sub=>sub.unsubscribe())
  }
  
  private getOnlineUsers(): void {
    this.subscripions.push(
      this.onlineUsersService.usersOnline.subscribe(res=>{
        this.usersOnline=res.filter(x=>x.id!=this.userID); 
    }))
  }

  getGameRequestsInv(user1:number): void {
    this.signalrService.hubConnection.invoke("GamesRequests", user1)
    .catch(err => console.error(err));
  }

  getGameRequestsList() {
    this.signalrService.hubConnection.on("GetGamesRequests", (users:any[]) => {
      this.gameRequests=users;
    });
  }

  addUserInv(user1:number,user2:number): void {
    this.signalrService.hubConnection.invoke("CreateLobby", user1, user2)
    .catch(err => console.error(err));
  }

  challangePlayer(user1:any,user2:any) {
    if (this.signalrService.hubConnection.state=="Connected") {
        this.addUserInv(user1,user2);
        this.getGameRequestsInv(user2);
    }
  }

  acceptGameRequest(lobbyID:any) {
    this.gameService.startGame(lobbyID).subscribe({
      next: res=> {
        this.gameService.startGameInv(res);        
      },
      error: err=> {
        console.log("neuspesno")
      }
    });
  }

  denyGameRequest(lobbyID:any) {
    this.gameService.denyGame(lobbyID).subscribe({
      next: res=> {
        this.getGameRequestsInv(this.userID);
      },
      error: err=> {
        console.log("neuspesno odbijen zahtev")
      }
    })
  }
}
