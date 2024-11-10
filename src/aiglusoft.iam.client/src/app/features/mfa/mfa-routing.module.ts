import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MfaSelectionComponent } from './mfa-selection/mfa-selection.component';
import { MfaVerificationComponent } from './mfa-verification/mfa-verification.component';
import { Fido2RegistrationComponent } from './fido2-registration/fido2-registration.component';
import { MfaManagementComponent } from './mfa-management/mfa-management.component';

const routes: Routes = [
  { path: 'select', component: MfaSelectionComponent },
  { path: 'verify', component: MfaVerificationComponent },
  { path: 'register-fido2', component: Fido2RegistrationComponent },
  { path: 'management', component: MfaManagementComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MfaRoutingModule { }
