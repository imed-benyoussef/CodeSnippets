import { concatLatestFrom } from '@ngrx/operators';import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import {
  verifyEmail,
  verifyEmailSuccess,
  verifyEmailFailure,
  setName,
  setGeneralInfo,
  setPassword,
  checkUserEmail,
  checkUserEmailSuccess,
  checkUserEmailFailure,
  completeSignup,
  signupSuccess,
  signupFailure,
} from '../actions/signup.actions';
import { SignupService } from '../../services/signup.service';
import { Store, select } from '@ngrx/store';
import { selectEncryptedPassword, selectUser } from '../selectors/signup.selectors';

import { JwtService } from '../../services/jwt.service';
import { TranslateService } from '@ngx-translate/core';


@Injectable()
export class SignupEffects {

  constructor(private actions$: Actions,
    private signupService: SignupService,
    private store: Store,
    private router: Router,
    private jwtService: JwtService,
    private translate: TranslateService
  ) { }


  goToGeneralInfo$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(setName),
      tap(() => this.router.navigate(['signup/general-info'], { relativeTo: this.router.routerState.root }))
    ) }, { dispatch: false }
  );

  goToEmail$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(setGeneralInfo),
      tap(() => this.router.navigate(['signup/email'], { relativeTo: this.router.routerState.root }))
    ) }, { dispatch: false }
  );


  checkUserData$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(checkUserEmail),
      concatLatestFrom(
        () => this.store.pipe(select(selectUser))
      ),
      exhaustMap(([action, user]) =>
        this.signupService.check({ firstName: user.firstName!, lastName: user.lastName!, birthDate: user.birthDate!, gender: user.gender!, email: action.email! })
          .pipe(
            exhaustMap((result) => {
              const user = this.jwtService.mapJwtToUserModel(result.id_token);
              return of(checkUserEmailSuccess({ token: result!, user: user! }));
            }),
            catchError(_ => of(checkUserEmailFailure({ errorKey: 'email', error: _.error })))
          ))
    ) }
  );



  goToEmailVerification$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(checkUserEmailSuccess),
      tap(() => this.router.navigate(['signup/email-verification']))
    ) }, { dispatch: false }
  );



  verifyEmail$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(verifyEmail),
      concatLatestFrom(
        () => this.store.pipe(select(selectUser))
      ),
      exhaustMap(([action, user]) =>
        this.signupService.verifyEmail({ email: user.email!, code: action.code }).pipe(
          exhaustMap((result) => {
            const user = this.jwtService.mapJwtToUserModel(result.id_token);
            const payload = { token: result!, user: user! };
            console.log(user);
            return of(verifyEmailSuccess(payload));
          }),
          catchError(_ => of(verifyEmailFailure({ errorKey: 'email-verification', error: _.error })))
        )
      )
    ) }
  );


  verifyEmailSuccess$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(verifyEmailSuccess),
      tap(() => this.router.navigate(['signup/password']))
    ) }, { dispatch: false }
  );



  gotoReviewAndTerms$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(setPassword),
      tap(() => this.router.navigate(['signup/review-and-terms']))
    ) }, { dispatch: false }
  );

  
  completeSignup$ = createEffect(() =>
    { return this.actions$.pipe(
      ofType(completeSignup),
      concatLatestFrom(
        () => this.store.pipe(select(selectEncryptedPassword))
      ),
      exhaustMap(([action, encryptedPassword]) =>
        this.signupService.password({ password: encryptedPassword! }).pipe(
          exhaustMap((result) => {
            const user = this.jwtService.mapJwtToUserModel(result.id_token);
            const payload = { token: result!, user: user! };
            this.router.navigate(['signup/initializing-account']);
            return of(signupSuccess());
          }),
          catchError(_ => of(signupFailure({ errorKey: 'password', error: _.error })))
        )
      )
    ) }
  );


}
