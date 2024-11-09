import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ForgotPasswordState } from '../reducers/forgot-password.reducer';

export const selectForgotPasswordState = createFeatureSelector<ForgotPasswordState>('forgotPassword');

export const selectForgotPasswordError = createSelector(
  selectForgotPasswordState,
  (state: ForgotPasswordState) => state.error
);

export const selectForgotPasswordSuccess = createSelector(
  selectForgotPasswordState,
  (state: ForgotPasswordState) => state.success
);
