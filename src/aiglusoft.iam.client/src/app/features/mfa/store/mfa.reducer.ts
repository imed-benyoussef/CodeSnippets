import { createReducer, on } from '@ngrx/store';
import * as MfaActions from './mfa.actions';

export interface MfaState {
  isVerified: boolean;
  error: any;
  // ...existing code...
}

export const initialState: MfaState = {
  isVerified: false,
  error: null,
  // ...existing code...
};

export const mfaReducer = createReducer(
  initialState,
  // ...existing code...
  on(MfaActions.activateMfaSuccess, state => ({
    ...state,
    error: null
  })),
  on(MfaActions.verifyMfaSuccess, state => ({
    ...state,
    isVerified: true,
    error: null
  })),
  on(MfaActions.mfaFailure, (state, { error }) => ({
    ...state,
    error
  }))
  // ...existing code...
);