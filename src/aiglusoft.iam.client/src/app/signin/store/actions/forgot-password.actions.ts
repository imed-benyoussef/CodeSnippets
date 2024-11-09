import { createAction, props } from '@ngrx/store';

export const forgotPassword = createAction(
  '[Forgot Password] Forgot Password',
  props<{ email: string }>()
);

export const forgotPasswordSuccess = createAction(
  '[Forgot Password] Forgot Password Success',
  props<{ message: string }>()
);

export const forgotPasswordFailure = createAction(
  '[Forgot Password] Forgot Password Failure',
  props<{ error: string }>()
);


export const clearForgotPasswordMessages = createAction(
  '[Forgot Password] Clear Forgot Password Messages'
);