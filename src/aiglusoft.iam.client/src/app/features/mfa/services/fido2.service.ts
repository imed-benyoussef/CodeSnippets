import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Fido2Service {
  private apiUrl = `/api/fido2`;

  constructor(private http: HttpClient) { }

  startRegistration(keyName: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register/options`, { keyName });
  }

  completeRegistration(attestation: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register/complete`, attestation);
  }
}
