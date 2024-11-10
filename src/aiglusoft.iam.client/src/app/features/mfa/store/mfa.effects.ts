import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as MfaActions from './mfa.actions';
import { MfaService } from '../mfa.service';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class MfaEffects {
  // ...existing code...

  constructor(
    private actions$: Actions,
    private mfaService: MfaService
  ) {}

  activateMfa$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MfaActions.activateMfa),
      mergeMap(action =>
        this.mfaService.activateMfa(action.method).pipe(
          map(() => MfaActions.activateMfaSuccess()),
          catchError(error => of(MfaActions.mfaFailure({ error })))
        )
      )
    )
  );

  verifyMfa$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MfaActions.verifyMfa),
      mergeMap(action =>
        this.mfaService.verifyMfa(action.code).pipe(
          map(() => MfaActions.verifyMfaSuccess()),
          catchError(error => of(MfaActions.mfaFailure({ error })))
        )
      )
    )
  );

  // ...existing code...
}