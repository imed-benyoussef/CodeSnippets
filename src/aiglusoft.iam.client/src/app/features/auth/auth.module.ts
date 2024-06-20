import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailVerificationComponent } from './components/email-verification/email-verification.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LockscreenComponent } from './components/lockscreen/lockscreen.component';
import { RecoverPasswordComponent } from './components/recover-password/recover-password.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';



@NgModule({
  declarations: [
    SignupComponent,
    SigninComponent,
    EmailVerificationComponent,
    ForgotPasswordComponent,
    LockscreenComponent,
    RecoverPasswordComponent,
    ResetPasswordComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([

      { path: 'signin', component: SigninComponent },
      { path: 'signup', component: SignupComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'email-verification', component: EmailVerificationComponent },
      { path: 'lockscreen', component: LockscreenComponent },
      { path: 'recover-password', component: RecoverPasswordComponent },
      { path: 'reset-password', component: ResetPasswordComponent },
      { path: '', redirectTo: 'signin', pathMatch: 'full' },
    ])
  ]
})
export class AuthModule { }
