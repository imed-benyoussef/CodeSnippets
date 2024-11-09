import { createReducer, on } from '@ngrx/store';

import * as models from '../../signup.models';
import { checkUserEmailFailure, checkUserEmailSuccess, initializeUser, setGeneralInfo, setName, setPassword, signupSuccess, verifyEmailFailure, verifyEmailSuccess } from '../actions/signup.actions';
export interface SignupState {

  token: models.Token | null,
  user: models.User;
  encryptedData: models.EncryptedData | null,
  termsAccepted: boolean;
  signupComplete: boolean;
  errors: {
    [key: string]: models.Error
  }

}

export const initialState: SignupState = {
  token: null,
  user: {},
  encryptedData: {},
  termsAccepted: false,
  signupComplete: false,
  errors: {}
};

const _signupReducer = createReducer(
  initialState,
  on(initializeUser, (state, { user }) => ({ ...state, user })),
  on(setName, (state, { firstName, lastName }) => ({ ...state, user: { ...state.user, firstName, lastName } })),
  on(setGeneralInfo, (state, { birthDate, gender }) => ({ ...state, user: { ...state.user, birthDate, gender } })),
  on(checkUserEmailSuccess, (state, action) => ({ ...state, token: action.token, user: action.user, errors: {} })),
  on(checkUserEmailFailure, (state, { errorKey, error }) => ({ ...state, user: { ...state.user, emailVerified: false }, errors: { ...state.errors, [errorKey]: error } })),
  on(verifyEmailSuccess, (state, action) => ({ ...state, token: action.token, user: action.user, errors: {} })),
  on(verifyEmailFailure, (state, { errorKey, error }) => ({ ...state, user: { ...state.user, emailVerified: false }, errors: { ...state.errors, [errorKey]: error } })),
  on(setPassword, (state, action) => ({ ...state, encryptedData: { ...state.encryptedData, encryptedPassword: action.encryptedPassword }, errors: {} })),
  // on(addPhoneNumberSuccess, (state, { phone}) => ({ ...state, user: { ...state.user, phoneNumber: phone }, errors: {} })),

  // on(setPassword, (state, { password, confirmPassword }) => ({ ...state, password, confirmPassword })),
  // on(setPhoneNumber, (state, { phoneNumber }) => ({ ...state, phoneNumber, phoneVerified: false })),
  // on(verifyPhoneCodeSuccess, (state) => ({ ...state, phoneVerified: true })),
  // on(acceptTerms, (state, { termsAccepted }) => ({ ...state, termsAccepted })),
  on(signupSuccess, (state) => ({ ...state, signupComplete: true, user: {}, encryptedData: {} })),
);

export function signupReducer(state: SignupState, action: any) {
  return _signupReducer(state, action);
}
