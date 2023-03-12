import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';
import { ConnectionService } from './services/connection.service';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'knights-and-diamonds';
  userID=this.authService?.userValue?.id

  constructor(public signalrService: SignalrService,
    public authService: AuthService,
    public connectionService: ConnectionService) { }

  ngOnInit() {
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
  }

  addConncectionInv(userID:any): void {
    this.signalrService.hubConnection.invoke("AddConnection",userID)
    .catch(err => console.error(err));
  }
}
