import { Injectable } from '@angular/core';
import { JwtService } from './jwt.service';

@Injectable({
  providedIn: 'root'
})
export class InitialStateService {
  constructor(private jwtService: JwtService) {}

  getInitialState(): any {
    const savedStateJSON = localStorage.getItem('signup');
    if (!savedStateJSON) return {};

    const savedState = JSON.parse(savedStateJSON);
    if (savedState && savedState.token && savedState.token.access_token) {
      const userData = this.jwtService.decodeToken(savedState.token.access_token);
      return {
        ...savedState,
        user: {
          ...savedState.user,
          ...userData
        }
      };
    }
    return savedState;
  }
}
