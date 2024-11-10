import { createAction, props } from '@ngrx/store';

// ...existing code...

export const activateMfa = createAction(
  '[MFA] Activer MFA',
  props<{ method: string }>()
);

export const activateMfaSuccess = createAction(
  '[MFA] Activation MFA Réussie'
);

export const verifyMfa = createAction(
  '[MFA] Vérifier MFA',
  props<{ code: string }>()
);

export const verifyMfaSuccess = createAction(
  '[MFA] Vérification MFA Réussie'
);

export const mfaFailure = createAction(
  '[MFA] Échec MFA',
  props<{ error: any }>()
);

// ...existing code...