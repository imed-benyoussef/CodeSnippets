import { createFeatureSelector, createSelector } from '@ngrx/store';
import { SignupState } from '../reducers/signup.reducer';
import { EncryptedData, Error } from '../../signup.models';

// Feature selector for signup state
export const selectSignupState = createFeatureSelector<SignupState>('signup');

// Individual selectors


export const selectUser = createSelector(
  selectSignupState,
  (state: SignupState) => state.user
);

export const selectEncryptedData = createSelector(
  selectSignupState,
  (state: SignupState) => state.encryptedData
);

export const selectEncryptedPassword = createSelector(
  selectSignupState,
  (state: SignupState) => state.encryptedData?.encryptedPassword
);

export const selectUserSet = createSelector(
  selectSignupState,
  (state: SignupState) => !!(state.user?.firstName && state.user?.lastName && state.user?.email && state.user?.userId)
);

export const selectNameSet = createSelector(
  selectSignupState,
  state => !!(state.user?.firstName && state.user?.lastName)
);

export const selectGeneralInfoSet = createSelector(
  selectSignupState,
  state => !!(state.user?.birthDate && state.user?.gender)
);

export const selectEmailSet = createSelector(
  selectSignupState,
  state => !!state.user?.email
);


export const selectAccessTokenSet = createSelector(
  selectSignupState,
  state => !!(state.token?.access_token)
);

export const selectemailVerificationSet = createSelector(
  selectSignupState,
  state => state.user?.emailVerified
);


export const selectAccessToken = createSelector(
  selectSignupState,
  state => state.token?.access_token
);


export const selectTermsAccepted = createSelector(
  selectSignupState,
  (state: SignupState) => state.termsAccepted
);

export const selectSignupComplete = createSelector(
  selectSignupState,
  (state: SignupState) => state.signupComplete
);

export const selectSignupErrors = createSelector(
  selectSignupState,
  (state: SignupState) => state.errors
);


export const selectSignupError = (key: string) => createSelector(
  selectSignupErrors,
  (errors: { [key: string]: Error }) => errors[key]
);
