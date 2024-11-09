import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { selectAccessToken } from '../store/selectors/signup.selectors';
import { Store } from '@ngrx/store';

@Injectable()
export class SignupService {

  private apiUrl = '/api/v1/'; // Replace with your actual API URL

  constructor(private http: HttpClient, private store: Store) { }


  check(payload: {
    lastName: string,
    firstName: string,
    birthDate: string,
    gender: string,
    email: string
  }
  ): Observable<any> {
    return this.http.post(`${this.apiUrl}register/check`, payload);
  }


  verifyEmail(payload: { email: string, code: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}register/email/verify`, payload);
  }


  password(payload: { password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}register/password`, payload);
  }



  addPhone(payload: { phone: string }): Observable<any> {

    return this.http.post(`${this.apiUrl}register/phone`, payload);
  }

  
  verifyPhone(payload: { phone: string, code: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}register/phone/verify`, payload);
  }

}
