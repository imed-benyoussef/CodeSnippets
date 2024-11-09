/*
*
Explanation
setName: This action is dispatched when the user sets their first and last names.
setGeneralInfo: This action is dispatched when the user sets their birth date and gender.
setEmail: This action is dispatched when the user sets their email address.
verifyEmail: This action is dispatched when the user verifies their email with a code.
setPassword: This action is dispatched when the user sets their password.
setPhoneNumber: This action is dispatched when the user sets their phone number.
verifyPhoneNumber: This action is dispatched when the user verifies their phone number with a code.
reviewInformation: This action is dispatched when the user reviews their information.
acceptTerms: This action is dispatched when the user accepts the terms and conditions.
completeSignup: This action is dispatched when the user completes the signup process.
signupSuccess: This action is dispatched when the signup process is successful.
signupFailure: This action is dispatched when there is a failure during the signup process.
*
*/

import { createAction, emptyProps, props } from '@ngrx/store';
import * as models from '../../signup.models';

export const initializeUser = createAction('[Signup] Initialize User', props<{ user: models.User }>());

// Actions for setting user name
export const setName = createAction(
  '[Signup] Set Name',
  props<{ firstName: string, lastName: string }>()
);

// Actions for setting general information
export const setGeneralInfo = createAction(
  '[Signup] Set General Info',
  props<{ birthDate: string, gender: string }>()
);

export const checkUserEmail = createAction(
  '[Signup] Check User Email',
  props<{ email: string }>()
);

export const checkUserEmailSuccess = createAction(
  '[Signup] Check User Email Success',
  props<{ token: models.Token, user: models.User }>()
);

export const checkUserEmailFailure = createAction(
  '[Signup] Check User Email Failure',
  props<{ errorKey: string, error: models.Error }>()
);



// Actions for Send Verification Code
export const verifyEmail = createAction(
  '[Signup] Verify Email',
  props<{code: string }>()
);

export const verifyEmailSuccess = createAction(
  '[Signup] Verify Email Success',
  props<{ token: models.Token, user: models.User }>()
);

export const verifyEmailFailure = createAction(
  '[Signup] Verify Email Failure',
  props<{ errorKey: string, error: models.Error }>()
);


// Actions for setting password
export const setPassword = createAction(
  '[Signup] Set Password',
  props<{ encryptedPassword: string }>()
);
export const setPasswordSuccess = createAction(
  '[Signup] Set Password Success',
  props<{ token: models.Token, user: models.User }>()
);
export const setPasswordFailure = createAction(
  '[Signup] Set Password Failure',
  props<{ errorKey: string, error: models.Error }>()
);


// Actions for setting phone number
export const skipPhoneNumber = createAction(
  '[Signup] Skip Phone Number',
  emptyProps
);

// Set phone number
export const addPhoneNumber = createAction(
  '[Signup] Add Phone Number',
  props<{ phone: string }>()
);

export const addPhoneNumberSuccess = createAction(
  '[Signup] Add Phone Number Success',
  props<{ phone: string }>()
);

export const addPhoneNumberFailure = createAction(
  '[Signup] Add Phone Number Failure',
  props<{ errorKey: string, error: models.Error }>()
);

// Send SMS verification code
export const sendSmsVerificationCode = createAction(
  '[Signup] Send SMS Verification Code',
  props<{ phoneNumber: string }>()
);

export const sendSmsVerificationCodeSuccess = createAction(
  '[Signup] Send SMS Verification Code Success'
);

export const sendSmsVerificationCodeFailure = createAction(
  '[Signup] Send SMS Verification Code Failure',
  props<{ error: any }>()
);

// Verify SMS code
export const verifyPhoneCode = createAction(
  '[Signup] Verify Phone Code',
  props<{ code: string }>()
);

export const verifyPhoneCodeSuccess = createAction(
  '[Signup] Verify Phone Code Success'
);

export const verifyPhoneCodeFailure = createAction(
  '[Signup] Verify Phone Code Failure',
  props<{ errorKey: string, error: models.Error }>()
);

// Actions for reviewing information
export const reviewInformation = createAction(
  '[Signup] Review Information'
);

// Actions for accepting terms
export const acceptTerms = createAction(
  '[Signup] Accept Terms',
  props<{ termsAccepted: boolean }>()
);

// Actions for completing signup
export const completeSignup = createAction(
  '[Signup] Complete Signup'
);

// Actions for signup success
export const signupSuccess = createAction(
  '[Signup] Signup Success'
);

// Actions for signup failure
export const signupFailure = createAction(
  '[Signup] Signup Failure',
  props<{ errorKey: string, error: models.Error }>()
);


export const unauthorizedError = createAction(
  '[Signup] Unauthorized Error'
);
