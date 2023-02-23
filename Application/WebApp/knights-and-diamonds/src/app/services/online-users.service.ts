import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { OnlineUsers, User } from 'src/classes/user';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class OnlineUsersService {
  usersOnline: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([])
  
  constructor(private signalrService: SignalrService) { }

  getOnlineUsersInv(): void {
    this.signalrService.hubConnection.invoke("GetOnlineUsers")
      .catch(err => console.error(err));
  }

  getOnlineUsersList(): void {
    this.signalrService.hubConnection.on("GetUsersFromHub", (users: Array<OnlineUsers>) => {
      this.usersOnline.next(users)
    });
  }
}
