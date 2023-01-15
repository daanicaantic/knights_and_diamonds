import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalrServiceService } from './services/SignalR/signalr-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'knights-and-diamonds';
  constructor( 
    public signalrService: SignalrServiceService
  ) 
  {}

  ngOnInit() {
    this.signalrService.startConnection();

    setTimeout(() => {
      this.signalrService.askServerListener();
      this.signalrService.askServer();
    }, 2000);
  }

  
  ngOnDestroy() {
    this.signalrService.hubConnection.off("askServerResponse");
  }
}
