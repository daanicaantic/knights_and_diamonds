import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
    ): Observable<HttpEvent<any>> {
      // add auth header with jwt if user is logged in and request is to api url
      const user = this.authenticationService.userValue;
      const isLoggedIn = user && user.token;
      if (isLoggedIn ) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${user.token}`,
          },
        });
      }
      return next.handle(request);
  }
}
