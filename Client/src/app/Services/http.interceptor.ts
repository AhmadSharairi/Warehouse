import { Injectable } from '@angular/core';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { NgToastService } from 'ng-angular-popup';
import { Router } from '@angular/router';

import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { LoginService } from '../Components/login/auth.service';
import { TokenModel } from '../shared/models/token-api.model';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private auth: LoginService,
    private toast: NgToastService,
    private roter: Router
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    // To send the token while interceptor to authorization for data First-step -->then step2 Config in the App.model in provider.
    const myToken = this.auth.getToken()!;
    if (myToken) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${myToken}` }, //  "Bearer"+mytoken
      });
    }
    //  catch error in interceptor when token is expired
    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            //  this.toast.warning({detail:"Warning" ,summary:"Token is expired, Please Login Again!"});
            // this.roter.navigate(['/'])
            return this.handleUnAuthorizedError(request, next);
          }
        }
        return throwError(() => new Error('Some other error occured!'));
      })
    );
  }

  handleUnAuthorizedError(req: HttpRequest<any>, next: HttpHandler) {
    let tokenApiModel = new TokenModel();
    tokenApiModel.accsessToken = this.auth.getToken()!;
    tokenApiModel.refreshToken = this.auth.getRefreshToken()!;
    return this.auth.renewToken(tokenApiModel).pipe(
      switchMap((data: TokenModel) => {
        this.auth.storeRefreshToken(data.refreshToken);
        this.auth.storeToken(data.accsessToken);
        req = req.clone({
          setHeaders: { Authorization: `Bearer ${data.accsessToken}` }, //  "Bearer"+mytoken
        });
        return next.handle(req);
      }),
      catchError((err) => {
        return throwError(() => {
          this.toast.warning('Token is expired, Please Login Again!', 'Warning', 5000);
          this.roter.navigate(['/']);
        });
      })
    );
  }
}
