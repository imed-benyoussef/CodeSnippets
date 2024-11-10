import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AuthActions from './auth.actions';
import { AuthService } from '../auth.service';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class AuthEffects {

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      mergeMap(action =>
        this.authService.login({ email: action.email, password: action.password }).pipe(
          map(response => AuthActions.loginSuccess({ token: response.token })),
          catchError(error => of(AuthActions.loginFailure({ error })))
        )
      )
    )
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      map(() => {
        this.authService.logout();
        return { type: '[Auth] Logout Success' };
      })
    )
  );

  constructor(
    private actions$: Actions,
    private authService: AuthService
  ) {}
}