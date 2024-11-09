import { createFeatureSelector, createSelector } from '@ngrx/store';
import { LoginState } from '../reducers/login.reducer';

export const selectSigninState = createFeatureSelector<LoginState>('login');

export const selectReturnUrl = createSelector(
  selectSigninState,
  (state: LoginState) => state.returnUrl
);

export const selectSigninError = createSelector(
  selectSigninState,
  (state: LoginState) => state.error
);