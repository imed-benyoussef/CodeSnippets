import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { User } from '../signup.models';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  decodeToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Failed to decode JWT:', error);
      return null;
    }
  }

  mapJwtToUserModel(token: string): User | null {
    const decodedToken = this.decodeToken(token);
    if (!decodedToken) {
      return null;
    }

    console.log("decodedToken" ,decodedToken )
    return {
      userId: decodedToken.sub,  // Standard JWT claim for user identifier
      firstName: decodedToken.given_name,
      lastName: decodedToken.family_name,
      birthDate: decodedToken.birthdate,  // Make sure this claim is included in your token
      gender: decodedToken.gender,  // Make sure this claim is included in your token
      email: decodedToken.email,
      emailVerified: decodedToken.email_verified === 'True',  // Make sure this claim is included in your token
      phoneNumber: decodedToken.phone_number,  // Make sure this claim is included in your token
      phoneVerified: decodedToken.phone_verified === 'True'  // Make sure this claim is included in your token
    };
  }
}
