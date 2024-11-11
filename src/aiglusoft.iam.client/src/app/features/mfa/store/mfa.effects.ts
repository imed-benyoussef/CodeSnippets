import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as MfaActions from './mfa.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { MfaService } from '../services/mfa.service';

@Injectable()
export class MfaEffects {
  constructor(
    private actions$: Actions,
    private mfaService: MfaService
  ) {}

  activateMfa$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MfaActions.activateMfa),
      mergeMap(({ method }) =>
        this.mfaService.activateMfa(method).pipe(
          map(() => MfaActions.activateMfaSuccess()),
          catchError(error => of(MfaActions.mfaFailure({ error })))
        )
      )
    )
  );

  verifyMfa$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MfaActions.verifyMfa),
      mergeMap(({ code }) =>
        this.mfaService.verifyMfa(code).pipe(
          map(() => MfaActions.verifyMfaSuccess()),
          catchError(error => of(MfaActions.mfaFailure({ error })))
        )
      )
    )
  );
}