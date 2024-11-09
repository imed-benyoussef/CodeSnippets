import { createActionGroup, createAction, props } from '@ngrx/store';

export const LoginActions = createActionGroup({
  source: 'Signin',
  events: {
    login: props<{ username: string; password: string; rememberMe: boolean; returnUrl: string }>(),
    loginSuccess: props<{ returnUrl: string }>(),
    loginFailure: props<{ error: string }>()
  }
});


export const checkLoginStatus = createAction(
  '[Auth] Check Login Status'
);

export const checkLoginStatusSuccess = createAction(
  '[Auth] Check Login Status Success',
  props<{ isLoggedIn: boolean }>()
);

export const checkLoginStatusFailure = createAction(
  '[Auth] Check Login Status Failure',
  props<{ error: any }>()
);
