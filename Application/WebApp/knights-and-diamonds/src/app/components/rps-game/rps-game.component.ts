import { Component, OnDestroy, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { timer } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { RpsGameService } from 'src/app/services/rps-game.service';

@Component({
  selector: 'app-rps-game',
  templateUrl: './rps-game.component.html',
  styleUrls: ['./rps-game.component.css']
})
export class RpsGameComponent implements OnInit, OnDestroy {

  rpsGameID: any;
  userID = this.authService?.userValue?.id;
  progressValue!: number;
  loadingBar: any;
  rpsGameStarted: Boolean = false;
  timer: any;
  timerCountDown: any;
  progress: any;
  player: any;
  playerFlag: any;
  winner: any;
  moveChosed: Boolean = false;

  constructor(private route: ActivatedRoute,
    private signalrService: SignalrService,
    private authService: AuthService,
    private rpsGameService: RpsGameService,
    private messageService: MessageService,) { }

  ngOnInit(): void {
    this.rpsGameID = this.route.snapshot.params['rpsGameID'];
    console.log(this.rpsGameID)
    this.progressValue = 0;
    this.timer = 10;
    this.getPlayer();
    this.getRPSWinner();
    this.progressionFunction();
  }

  ngOnDestroy(): void {
    clearInterval(this.progress)
    this.rpsGameService.removeUserFromUsersInGame(this.userID).subscribe({});
  }

  getPlayer() {
    this.rpsGameService.getPlayer(this.rpsGameID, this.userID).subscribe({
      next: res => {
        this.player = res;
        console.log(this.player)
        if (this.player.id % 2 == 0) {
          this.playerFlag = "Player2"
        }
        else {
          this.playerFlag = "Player1"
        }
        console.log(this.playerFlag)
      },
      error: err => {
        console.log(err)
      }
    })
  }

  chooseRPSMove(move: string) {
    this.rpsGameService.playRPSMove(this.player.id, move).subscribe({
      next: (res: any) => {
        this.moveChosed == true;
        this.messageService.add({ key: 'br', severity: 'success', summary: 'Uspešno', detail: 'Uspesno odigran potez!' });
      },
      error: err => {
        this.messageService.add({ key: 'br', severity: 'error', summary: 'Neuspešno', detail: err.error });
      }
    });
  }

  checkRPSWinnerInv(): void {
    this.signalrService.hubConnection.invoke("CheckRPSWinner", this.player.rpsGameID)
      .catch(err => console.error(err));
  }

  getRPSWinner() {
    this.signalrService.hubConnection.on("GetRPSWinner", (winner: any) => {
      this.winner = winner;
    });
  }

  progressionFunction() {
    this.progressValue = 0;
    let progressEndValue = 100;
    let speed = 50;

    this.progress = setInterval(() => {
      this.progressValue++;
      this.loadingBar = 'conic-gradient(red ' + this.progressValue * 3.6 + 'deg, orange ' + this.progressValue * 3.6 + 'deg)';
      if (this.progressValue == progressEndValue) {
        clearInterval(this.progress);
        this.timerFunction();
      }
      speed = 50;
    }, speed);
  }

  timerFunction() {
    clearInterval(this.progress);
    this.timer = 10;
    console.log(this.timer)
    this.rpsGameStarted = true;
    this.progressValue = 0;
    this.progress = setInterval(() => {
      this.timer--;
      this.timerCountDown = this.timer * 3.3333333333333333333333 + "%"
      if (this.timer == this.progressValue) {
        clearInterval(this.progress);
        this.checkRPSWinnerInv();
      }
    }, 1000)
  }
}
