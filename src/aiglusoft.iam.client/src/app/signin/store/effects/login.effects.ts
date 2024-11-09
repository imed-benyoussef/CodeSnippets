import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import {checkLoginStatus, checkLoginStatusFailure, checkLoginStatusSuccess, LoginActions} from '../actions/login.actions';

@Injectable()
export class LoginEffects {

  checkLoginStatus$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(checkLoginStatus),
      mergeMap(() =>
        this.http.get<{ message: string }>('/api/v1/account/login').pipe(
          map(response => {
            // this.router.navigate(['/dashboard']);
            return checkLoginStatusSuccess({ isLoggedIn: true });
          }),
          catchError(error => {
            // this.router.navigate(['signin/login']);
            return of(checkLoginStatusFailure({ error }));
          })
        )
      )
    ) }
  );

  login$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(LoginActions.login),
      mergeMap((action) =>
        this.http.post(`api/v1/account/login`, action).pipe(
          map(() => LoginActions.loginSuccess({ returnUrl: action.returnUrl })),
          catchError((error) => of(LoginActions.loginFailure({ error: error.error.error_description })))
        )
      )
    ) }
  );

  loginSuccess$ = createEffect(
    () =>
      { return this.actions$.pipe(
        ofType(LoginActions.loginSuccess),
        tap((action) => {
          if (action.returnUrl) {
            window.location.href = action.returnUrl;
          } else {
            this.router.navigate(['/dashboard']);
          }
        })
      ) },
    { dispatch: false }
  );

  

  constructor(
    private actions$: Actions,
    private http: HttpClient,
    private router: Router
  ) {}
}