import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MockSignupService {


  sendVerificationCode(email: string): Observable<any> {

    console.log('Mock sendVerificationCode called with email:', email);

    // Simulate a successful response
    return of({ success: true });
  }

  verifyEmail(verificationCode: string): Observable<any> {
    console.log('Mock verifyEmail called with verificationCode:', verificationCode);

    // Simulate a successful response
    return of({ success: true });
  }

  verifyPhoneNumber(verificationCode: string): Observable<any> {
    console.log('Mock verifyPhoneNumber called with verificationCode:', verificationCode);

    // Simulate a successful response
    return of({ success: true });
  }

  completeSignup(signupData: any): Observable<any> {
    console.log('Mock completeSignup called with signupData:', signupData);

    // Simulate a successful response
    return of({ success: true });
  }

  sendSmsVerificationCode(phoneNumber: string) : Observable<any> {
    return of({ success: true });
  }
  

  verifyPhoneCode(verificationCode: string) : Observable<any> {
    return of({ success: true });
  }
  
  
}
