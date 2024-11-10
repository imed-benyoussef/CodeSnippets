import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authUrl = '/api/auth';
  private tokenSubject = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) { }

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post(`${this.authUrl}/login`, credentials).pipe(
      tap((response: any) => {
        this.setToken(response.token);
      })
    );
  }

  signUp(userData: any): Observable<any> {
    return this.http.post(`${this.authUrl}/sign-up`, userData);
  }

  resetPassword(email: string): Observable<any> {
    return this.http.post(`${this.authUrl}/reset-password`, { email });
  }

  logout(): void {
    this.clearToken();
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  private setToken(token: string): void {
    localStorage.setItem('access_token', token);
    this.tokenSubject.next(token);
  }

  private clearToken(): void {
    localStorage.removeItem('access_token');
    this.tokenSubject.next(null);
  }
}
