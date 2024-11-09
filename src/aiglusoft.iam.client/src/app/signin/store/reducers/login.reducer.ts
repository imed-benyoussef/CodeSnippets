import { createReducer, on } from '@ngrx/store';
import {LoginActions} from '../actions/login.actions';

export interface LoginState {
  returnUrl: string | null;
  error: string | null;
}

export const initialState: LoginState = {
  returnUrl: null,
  error: null,
};

export const loginReducer = createReducer(
  initialState,
  on(LoginActions.loginSuccess, (state, { returnUrl }): LoginState => ({
    ...state,
    returnUrl,
    error: null,
  })),
  on(LoginActions.loginFailure, (state, { error }): LoginState => ({
    ...state,
    error,
  })))
