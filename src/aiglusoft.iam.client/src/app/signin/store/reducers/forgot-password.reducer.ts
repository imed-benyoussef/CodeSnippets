import { createReducer, on } from '@ngrx/store';
import * as ForgotPasswordActions from '../actions/forgot-password.actions';

export interface ForgotPasswordState {
  error: string | null;
  success: string | null;
}

export const initialState: ForgotPasswordState = {
  error: null,
  success: null
};

export const forgotPasswordReducer = createReducer<ForgotPasswordState>(
  initialState,
  on(ForgotPasswordActions.forgotPassword, (state):ForgotPasswordState => ({
    ...state,
    error: null,
    success: null
  })),
  on(ForgotPasswordActions.forgotPasswordSuccess, (state, action): ForgotPasswordState => ({
    ...state,
    success: action.message
  })),
  on(ForgotPasswordActions.forgotPasswordFailure, (state, action): ForgotPasswordState => ({
    ...state,
    error: action.error
  })),
  on(ForgotPasswordActions.clearForgotPasswordMessages, (state): ForgotPasswordState => ({
    ...state,
    error: null,
    success: null,
  }))
);
