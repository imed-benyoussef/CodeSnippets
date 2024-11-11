import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface MfaMethod {
  type: 'totp' | 'sms' | 'fido2';
  isEnabled: boolean;
  lastUsed?: Date;
  identifier?: string; // Phone number for SMS, key handle for FIDO2
}

@Injectable({
  providedIn: 'root'
})
export class MfaService {
  private apiUrl = '/api/mfa';

  constructor(private http: HttpClient) {}

  getMfaStatus(): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/status`);
  }

  toggleMfa(enable: boolean): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/toggle`, { enable });
  }

  setupTotp(): Observable<{ qrCodeUrl: string, secret: string }> {
    return this.http.post<{ qrCodeUrl: string, secret: string }>(`${this.apiUrl}/setup-totp`, {});
  }

  setupSms(phoneNumber: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/setup-sms`, { phoneNumber });
  }

  getEnabledMethods(): Observable<MfaMethod[]> {
    return this.http.get<MfaMethod[]>(`${this.apiUrl}/methods`);
  }

  setupFido2(): Observable<{ challenge: string }> {
    return this.http.post<{ challenge: string }>(`${this.apiUrl}/setup-fido2`, {});
  }

  verifyFido2(response: any): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/verify-fido2`, response);
  }

  activateMfa(method: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/activate`, { method });
  }

  verifyMfa(code: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/verify`, { code });
  }
}