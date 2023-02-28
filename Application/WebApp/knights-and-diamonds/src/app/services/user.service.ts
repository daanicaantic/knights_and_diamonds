import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/classes/user';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  addUser(user: any): any {
    return this.httpClient.post(`https://localhost:7250/User/AddUser`, user);
  }

  getUser(userID: number): Observable<User[]> {
    return this.httpClient.get<User[]>(`https://localhost:7250/User/GetUser?id=${userID}`)
  }
}
