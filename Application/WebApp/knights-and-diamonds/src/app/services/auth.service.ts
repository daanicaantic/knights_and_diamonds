import { Injectable } from '@angular/core';
//import { HttpClient } from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public userSubject!: BehaviorSubject<any>
  public user!: Observable<any>

  constructor(private http: HttpClient) { 
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
      localStorage.removeItem('user');
      this.userSubject.next(null);
  }

  public get userValue(): any {
    return this.userSubject.value;
  }
  
  loginStatusChange(): Observable<boolean> {
      return this.userSubject.asObservable();
  }
}
