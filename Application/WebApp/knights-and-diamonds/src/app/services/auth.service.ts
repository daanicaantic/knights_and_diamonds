import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { SignalrService } from './signalr.service';
import { OnlineUsers, User } from 'src/classes/user';
import { OnlineUsersService } from './online-users.service';
import { ConnectionService } from './connection.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userSubject!: BehaviorSubject<any>
  public user!: Observable<any>
  private router!: Router
  private us!:any
  usersOnline:Array<OnlineUsers>=new Array<OnlineUsers>();
  subscripions: Subscription[] = []

  constructor(
    private http: HttpClient,
    private signalrService:SignalrService,
    private onlineUsersService:OnlineUsersService,
    private connectionService: ConnectionService) { 
    
    this.userSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('user') || '{}')
    );
    this.user = this.userSubject.asObservable();
  }

  login(userData: any) {
    return this.http.post(`https://localhost:7250/LogIn`, userData)
    .pipe(map((user:any)=>{
      console.log('LOGOVANSAM')
      console.log(user)
      localStorage.setItem('user',JSON.stringify(user))
      this.userSubject.next(user)
      this.connectionService.addConnectionInv(this.userValue.id);
    }))
  }

  logout() {
    this.http.delete(`https://localhost:7250/LogOut?userID=${this.userValue.id}`).subscribe({
      next: res=>{
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.onlineUsersService.getOnlineUsersInv();
        this.onlineUsersService.getOnlineUsersList();
      },
      error: err=>{
      }
    });
  }

  public get userValue(): any {
    return this.userSubject.value;
  }
  
  loginStatusChange(): Observable<boolean> {
      return this.userSubject.asObservable();
  }

  getOnlineUsersList()
  {
    this.signalrService.hubConnection.on("GetUserToken", (users: any) => {
      this.us=users;
      });
  }
}
