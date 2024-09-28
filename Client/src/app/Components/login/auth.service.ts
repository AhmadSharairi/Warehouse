import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenModel } from '../../shared/models/token-api.model';
import { ResetPassword } from './../../shared/models/ResetPassword';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: string = 'http://localhost:5152/api/Auth/'; // connect to the Api
  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) {
    this.userPayload = this.decodeToken();
  }

  // send to the backend result Obj

  loginPost(loginObj: any) {
    return this.http.post<any>(`${this.baseUrl}login`, loginObj); // post to the  authenticate Api
  }

  // **********************Token functions***********************

  // store Token From BACKEND
  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }
  //get Token From BACKEND

  getToken() {
    const tokenHere = localStorage.getItem('token');
    return localStorage.getItem('token');
  }

  // store Refresh Token from Backend
  storeRefreshToken(tokenValue: string) {
    localStorage.setItem('refreshToken', tokenValue);
  }

  //Get Refresh Token From Backend
  getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }

  //is Logged In the user or not
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); //if user have token that make the user is loggedIn and have token, each user have diffrent token
  }

  // **********************Token functions************************

  SignOut() {
    localStorage.clear();
    this.router.navigate(['/']);
  }

  // Decode Token role and email from before that must be this command -- > npm i @auth0/angular-jwt
  decodeToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log("here is the data user");
    console.log(jwtHelper.decodeToken(token));

    return jwtHelper.decodeToken(token); // return payload data as obj from token
  }

  renewToken(tokenApi: TokenModel)
  {
    return this.http.post<any>(`${this.baseUrl}refresh`, tokenApi);
  }

  getRoleFromToken()
  {
    if (this.userPayload) return this.userPayload.role;
  }


  resetPassword(resetPassword: ResetPassword)
  {
    console.log(resetPassword);

    return this.http.post<any>(`${this.baseUrl}reset-password`, resetPassword);
  }
}
