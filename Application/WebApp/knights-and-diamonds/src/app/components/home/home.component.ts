import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, elementAt, Observable, Subject, Subscription } from 'rxjs';
import { OnlineUsers } from 'src/classes/user';
import { AuthService } from '../../services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { OnlineUsersService } from 'src/app/services/online-users.service';
import { Router } from '@angular/router';
import { RpsGameService } from 'src/app/services/rps-game.service';

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
    private onlineUsersService:OnlineUsersService,
    private rpsGameService:RpsGameService,
    private router: Router
    ) {}
  
  ngOnInit(): void {
    this.onlineUsersService.getOnlineUsersList();
    this.getOnlineUsers();
    this.getGameRequestsList();
    this.rpsGameService.startGameResponse();
 
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
    this.signalrService.hubConnection.on("GetGamesRequests", (gameRequests:any[]) => {
      this.gameRequests=gameRequests;
      console.log(gameRequests);
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
    this.rpsGameService.startGame(lobbyID).subscribe({
      next: res=> {
        this.rpsGameService.startGameInv(res);
      },
      error: err=> {
        console.log("neuspesno")
      }
    });
  }

  denyGameRequest(lobbyID:any) {
    this.rpsGameService.denyGame(lobbyID).subscribe({
      next: res=> {
        this.getGameRequestsInv(this.userID);
      },
      error: err=> {
        console.log("neuspesno odbijen zahtev")
      }
    })
  }
}
