
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Fido2Service {
  private baseUrl = 'api/fido2';

  constructor(private http: HttpClient) {}

  startRegistration(keyName: string): Observable<PublicKeyCredentialCreationOptions> {
    return this.http.post<PublicKeyCredentialCreationOptions>
      (`${this.baseUrl}/register/options`, { keyName });
  }

  completeRegistration(credential: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register/complete`, credential);
  }
}