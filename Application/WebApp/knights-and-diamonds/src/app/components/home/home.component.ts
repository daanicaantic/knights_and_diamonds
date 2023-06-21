import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  BehaviorSubject,
  elementAt,
  Observable,
  Subject,
  Subscription,
} from 'rxjs';
import { OnlineUsers } from 'src/classes/user';
import { AuthService } from '../../services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { OnlineUsersService } from 'src/app/services/online-users.service';
import { Router } from '@angular/router';
import { RpsGameService } from 'src/app/services/rps-game.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  public message: string = '';
  public messages: string[] = [];
  usersOnline: Array<OnlineUsers> = new Array<OnlineUsers>();
  subscripions: Subscription[] = [];
  userID = this.authService?.userValue?.id;
  gameRequests!: any[];
  gameRequested!: any[];
  constructor(
    private authService: AuthService,
    private signalrService: SignalrService,
    private onlineUsersService: OnlineUsersService,
    private rpsGameService: RpsGameService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getOnlineUsersList();
    this.getGameRequestsList();
    this.startGameResponse();

    if (this.userID != undefined) {
      this.removeUserFromGame();
      if (this.signalrService.hubConnection.state == 'Connected') {
        this.onlineUsersService.getOnlineUsersInv();
      } else {
        this.subscripions.push(
          this.signalrService.ssSubj.subscribe((obj: any) => {
            if (obj.type == 'HubConnStarted') {
              this.onlineUsersService.getOnlineUsersInv();
            }
          })
        );
      }
    }
  }

  getOnlineUsersList(): void {
    this.signalrService.hubConnection.on(
      'GetUsersFromHub',
      (users: Array<OnlineUsers>) => {
        this.usersOnline = users.filter((x) => x.id != this.userID);
        this.getGameRequestsInv(this.userID);
      }
    );
  }

  getGameRequestsInv(user1: number): void {
    this.signalrService.hubConnection
      .invoke('GamesRequests', user1)
      .catch((err) => console.error(err));
  }

  getGameRequestsList() {
    this.signalrService.hubConnection.on(
      'GetGamesRequests',
      (gameRequests: any[], gameRequested: any[]) => {
        this.gameRequests = gameRequests;
        this.gameRequested = gameRequested;
        console.log('game request list', gameRequests);
        console.log('game request list', gameRequested);

        this.gameRequests.forEach((lobby: any) => {
          if (this.usersOnline.indexOf(lobby.user1) == -1) {
            console.log(this.usersOnline.indexOf(lobby.user1) == -1);
            var onlineUser = this.usersOnline.find(
              (x) => x.id == lobby.user1.id
            );
            onlineUser!.lobbyID = lobby.id;
            onlineUser!.status = 1;
          }
        });

        this.gameRequested.forEach((lobby: any) => {
          var onlineUser = this.usersOnline.find((x) => x.id == lobby.user2.id);
          onlineUser!.status = 0;
        });
      }
    );
  }

  addUserInv(user1: number, user2: number): void {
    this.signalrService.hubConnection
      .invoke('CreateLobby', user1, user2)
      .catch((err) => console.error(err));
  }

  challangePlayer(user1: any, user2: any) {
    if (this.signalrService.hubConnection.state == 'Connected') {
      this.addUserInv(user1, user2);
      this.onlineUsersService.getOnlineUsersInv();
    }
  }

  acceptGameRequest(lobbyID: any) {
    this.subscripions.push(
      this.rpsGameService.startGame(lobbyID).subscribe({
        next: (res) => {
          // this.onlineUsersService.getOnlineUsersInv();
          console.log(lobbyID);
          this.rpsGameService.startGameInv(res);
        },
        error: (err) => {
          console.log('neuspesno');
        },
      })
    );
  }

  denyGameRequest(lobbyID: any) {
    this.subscripions.push(
      this.rpsGameService.denyGame(lobbyID).subscribe({
        next: (res) => {
          this.onlineUsersService.getOnlineUsersInv();
        },
        error: (err) => {
          console.log('neuspesno odbijen zahtev');
        },
      })
    );
  }
  startGameResponse() {
    this.signalrService.hubConnection.on('RPSGameStarted', (rpsGameID: any) => {
      this.router.navigate(['/rpsGame', rpsGameID]);
    });
  }
  removeUserFromGame() {
    this.subscripions.push(
      this.rpsGameService.removeUserFromUsersInGame(this.userID).subscribe({
        next: (res: any) => {
          console.log('res', res);
        },
        error: (err) => {
          console.log(err.error);
        },
      })
    );
  }
  ngOnDestroy(): void {
    this.subscripions.forEach((sub) => sub.unsubscribe());
    this.signalrService.hubConnection.off('GetUsersFromHub');
    this.signalrService.hubConnection.off('GetGamesRequests');
    this.signalrService.hubConnection.off('RPSGameStarted');
  }
}
