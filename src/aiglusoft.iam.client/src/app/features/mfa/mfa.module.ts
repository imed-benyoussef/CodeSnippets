import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MfaRoutingModule } from './mfa-routing.module';

import { MfaSelectionComponent } from './mfa-selection/mfa-selection.component';
import { MfaVerificationComponent } from './mfa-verification/mfa-verification.component';
import { Fido2RegistrationComponent } from './fido2-registration/fido2-registration.component';

import { ReactiveFormsModule } from '@angular/forms';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { mfaReducer } from './store/mfa.reducer';
import { MfaEffects } from './store/mfa.effects';

@NgModule({
  declarations: [
    MfaSelectionComponent,
    MfaVerificationComponent,
    Fido2RegistrationComponent
  ],
  imports: [
    CommonModule,
    MfaRoutingModule,
    ReactiveFormsModule,
    StoreModule.forFeature('mfa', mfaReducer),
    EffectsModule.forFeature([MfaEffects])
  ]
})
export class MfaModule { }
