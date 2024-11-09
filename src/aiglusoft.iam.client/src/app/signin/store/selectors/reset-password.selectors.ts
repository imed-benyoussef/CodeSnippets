// src/app/signin/store/selectors/reset-password.selectors.ts
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ResetPasswordState } from '../reducers/reset-password.reducer';

export const selectResetPasswordState = createFeatureSelector<ResetPasswordState>('resetPassword');

export const selectResetPasswordLoading = createSelector(
  selectResetPasswordState,
  (state: ResetPasswordState) => state.loading
);

export const selectResetPasswordError = createSelector(
  selectResetPasswordState,
  (state: ResetPasswordState) => state.error
);
