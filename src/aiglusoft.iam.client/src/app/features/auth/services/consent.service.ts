
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

@Injectable({
  providedIn: 'root'
})
export class ConsentService {
  constructor(private http: HttpClient) {}

  async getClientInfo(clientId: string) {
    return this.http.get(`${environment.apiUrl}/clients/${clientId}`).toPromise();
  }

  async giveConsent(consentData: { scopes: string[], remember: boolean }) {
    return this.http.post(`${environment.apiUrl}/consent`, consentData).toPromise();
  }

  async denyConsent() {
    return this.http.post(`${environment.apiUrl}/consent/deny`, {}).toPromise();
  }

  async hasStoredConsent(clientId: string, scopes: string[]) {
    return this.http.get(`${environment.apiUrl}/consent/check`, {
      params: { client_id: clientId, scope: scopes.join(' ') }
    }).toPromise();
  }
}