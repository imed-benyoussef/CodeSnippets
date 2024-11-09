import { Routes } from '@angular/router';
import { SigninComponent } from './signin.component';
import { LoginPageComponent } from './containers/login-page/login-page.component';
import { ForgotPasswordPageComponent } from './containers/forgot-password-page/forgot-password-page.component';
import { ResetPasswordPageComponent } from './containers/reset-password-page/reset-password-page.component';

export const signinRoutes: Routes = [{
  path: '', component: SigninComponent, children: [
    { path: 'login', component: LoginPageComponent  },
    { path: 'forgot-password', component: ForgotPasswordPageComponent },
    { path: 'reset-password', component: ResetPasswordPageComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
  ],
  
}

];