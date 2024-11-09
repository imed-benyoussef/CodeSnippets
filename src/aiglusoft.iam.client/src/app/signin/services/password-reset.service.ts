// src/app/signin/services/password-reset.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class PasswordResetService {
  private apiUrl = ''; // Remplacez par votre URL API

  constructor(private http: HttpClient) {}

  resetPassword(token: string, newPassword: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/api/v1/account/reset-password`, { token, newPassword }, { headers });
  }
}
