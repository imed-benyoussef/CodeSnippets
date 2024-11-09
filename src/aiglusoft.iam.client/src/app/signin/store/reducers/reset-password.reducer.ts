// src/app/signin/store/reducers/reset-password.reducer.ts
import { createReducer, on } from '@ngrx/store';
import * as ResetPasswordActions from '../actions/reset-password.actions';

export interface ResetPasswordState {
  loading: boolean;
  error: string | null;
}

export const initialState: ResetPasswordState = {
  loading: false,
  error: null
};

const _resetPasswordReducer = createReducer(
  initialState,
  on(ResetPasswordActions.resetPassword, state => ({ ...state, loading: true })),
  on(ResetPasswordActions.resetPasswordSuccess, state => ({ ...state, loading: false })),
  on(ResetPasswordActions.resetPasswordFailure, (state, { error }) => ({ ...state, loading: false,  error }))
);

export function resetPasswordReducer(state: ResetPasswordState | undefined, action: any) {
  return _resetPasswordReducer(state, action);
}
