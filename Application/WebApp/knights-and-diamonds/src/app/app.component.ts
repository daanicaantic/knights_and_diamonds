import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';
import { ConnectionService } from './services/connection.service';
import { SignalrService } from './services/signalr.service';
import { IngameService } from './services/ingame.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'knights-and-diamonds';
  userID=this.authService?.userValue?.id
  gameStatus?=true;

  constructor(
    public signalrService: SignalrService,
    public inGameService: IngameService,
    public authService: AuthService,
    public connectionService: ConnectionService,
    private cdr:ChangeDetectorRef) { }

  ngOnInit() {
    this.checkGameStatus();

    this.signalrService.startConnection();
    if(this.userID!=undefined){
      if (this.signalrService.hubConnection.state=="Connected") {
        this.connectionService.addConnectionInv(this.userID);
      }
      else{
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == "HubConnStarted") {
            this.connectionService.addConnectionInv(this.userID);
          }
        });
      }
    }
  }

  ngOnDestroy() {
    this.signalrService.hubConnection.off("askServerResponse");
    // this.inGameService.unsubscribe();
  }

  addConncectionInv(userID:any): void {
    this.signalrService.hubConnection.invoke("AddConnection",userID)
    .catch(err => console.error(err));
  }
  checkGameStatus(){
    this.inGameService.gameStarted.subscribe((obj: any) => {
      if (obj.type == "GameOn") {
        this.gameStatus=false;
      }
      else{
        this.gameStatus=true;
      }
      this.cdr.detectChanges();
    });
  }
}
