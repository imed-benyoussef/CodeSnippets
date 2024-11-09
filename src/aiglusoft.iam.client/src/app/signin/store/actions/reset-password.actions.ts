// src/app/signin/store/actions/reset-password.actions.ts
import { createAction, props } from '@ngrx/store';

export const resetPassword = createAction(
  '[Reset Password Page] Reset Password',
  props<{ token: string, password: string }>()
);

export const resetPasswordSuccess = createAction(
  '[Reset Password API] Reset Password Success'
);

export const resetPasswordFailure = createAction(
  '[Reset Password API] Reset Password Failure',
  props<{ error: string }>()
);
