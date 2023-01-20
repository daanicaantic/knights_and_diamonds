import { Injectable } from '@angular/core';
//import { HttpClient } from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { HomeComponent } from '../home/home.component';
import { Router } from '@angular/router';
import { OnelineusersService } from './onelineusers.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userSubject!: BehaviorSubject<any>
  public user!: Observable<any>
  private router!: Router



  constructor(private http: HttpClient,
              public onelineusers:OnelineusersService) { 
    
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
        

    }))
  }

  logout() {
    this.http.delete(`https://localhost:7250/LogOut?userID=${this.userValue.id}`).subscribe({
      next: res=>{
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.onelineusers.startConnection();

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
}
