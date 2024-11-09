import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { catchError, map, mergeMap, of } from 'rxjs';
import * as ForgotPasswordActions from '../actions/forgot-password.actions';

@Injectable()
export class ForgotPasswordEffects {
  forgotPassword$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(ForgotPasswordActions.forgotPassword),
      mergeMap(action =>
        this.http.post('api/v1/account/forgot-password', { email: action.email }).pipe(
          map(() => ForgotPasswordActions.forgotPasswordSuccess({ message: 'Password recovery email sent successfully.' })),
          catchError(error => of(ForgotPasswordActions.forgotPasswordFailure({ error: error.error.error_description || 'Failed to send password recovery email.' })))
        )
      )
    )
  });

  constructor(private actions$: Actions, private http: HttpClient) { }
}
