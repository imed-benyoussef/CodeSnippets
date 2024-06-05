import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninComponent } from './features/auth/components/signin/signin.component';
import { SignupComponent } from './features/auth/components/signup/signup.component';
import { EmailVerificationComponent } from './features/auth/components/email-verification/email-verification.component';
import { ForgotPasswordComponent } from './features/auth/components/forgot-password/forgot-password.component';
import { LockscreenComponent } from './features/auth/components/lockscreen/lockscreen.component';
import { RecoverPasswordComponent } from './features/auth/components/recover-password/recover-password.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';

const routes: Routes = [
   // Auth routes
   { path: '',  loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) },
   // Lazy-loaded feature module routes
  //  { path: 'dashboard', canActivate: [AuthGuard], loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule) },
   // Redirect to signin by default
   { path: '', redirectTo: '/signin', pathMatch: 'full' },
   // Wildcard route for a 404 page (not found)
   { path: '**', redirectTo: '/signin' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
