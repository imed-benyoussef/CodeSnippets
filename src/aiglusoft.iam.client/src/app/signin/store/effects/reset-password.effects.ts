// src/app/signin/store/effects/reset-password.effects.ts
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { PasswordResetService } from '../../services/password-reset.service';
import { resetPassword, resetPasswordFailure, resetPasswordSuccess } from '../actions/reset-password.actions';
import { Router } from '@angular/router';

@Injectable()
export class ResetPasswordEffects {
    resetPassword$ = createEffect(() =>
        { return this.actions$.pipe(
            ofType(resetPassword),
            switchMap(action =>
                this.passwordResetService.resetPassword(action.token, action.password).pipe(
                    map(() => resetPasswordSuccess()),
                    catchError(error => of(resetPasswordFailure({ error: error.error.error_description })))
                )
            )
        ) }
    );

    resetPasswordSuccess$ = createEffect(() =>
        { return this.actions$.pipe(
            ofType(resetPasswordSuccess),
            tap(() => this.router.navigate(['signin/login']))
        ) }, { dispatch: false }
    );

    constructor(
        private router: Router,
        private actions$: Actions,
        private passwordResetService: PasswordResetService
    ) { }
}
