import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { SessionManagementComponent } from './session-management/session-management.component';
import { SecuritySettingsComponent } from './security-settings/security-settings.component';


@NgModule({
  declarations: [
    ChangePasswordComponent,
    SessionManagementComponent,
    SecuritySettingsComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule
  ]
})
export class AccountModule { }
