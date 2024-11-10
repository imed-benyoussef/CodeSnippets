import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MfaService {
  private mfaUrl = '/api/mfa';
  private mfaVerifiedSubject = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) { }

  activateMfa(method: string): Observable<any> {
    return this.http.post(`${this.mfaUrl}/activate`, { method });
  }

  verifyMfa(code: string): Observable<any> {
    return this.http.post(`${this.mfaUrl}/verify`, { code }).pipe(
      tap(() => {
        this.setMfaVerified(true);
      })
    );
  }

  isMfaVerified(): boolean {
    return this.mfaVerifiedSubject.value;
  }

  private setMfaVerified(verified: boolean): void {
    this.mfaVerifiedSubject.next(verified);
  }
}
