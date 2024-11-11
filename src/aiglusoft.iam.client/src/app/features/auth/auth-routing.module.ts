import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { ConsentComponent } from './consent/consent.component';

const routes: Routes = [
  { path: 'signin', component: LoginComponent },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'password-reset', component: PasswordResetComponent },
  { path: 'consent', component: ConsentComponent },
  { path: '', redirectTo: 'signin', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
