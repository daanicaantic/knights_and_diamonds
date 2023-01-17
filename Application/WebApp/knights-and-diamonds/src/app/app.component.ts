import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'knights-and-diamonds';
  constructor( 
    public signalr: SignalrService
  ) 
  {}

  ngOnInit() {
    this.signalr.startConnection();

    setTimeout(() => {
      this.signalr.askServerListener();
      this.signalr.askServer();
    }, 2000);
  }

  
  ngOnDestroy() {
    this.signalr.hubConnection.off("askServerResponse");
  }
}
