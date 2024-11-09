import { NgModule } from '@angular/core';
import { APP_BASE_HREF, CommonModule } from '@angular/common';

import { signinRoutes } from './signin.routes';
import { SharedModule } from '@shared';
import { SigninComponent } from './signin.component';

import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RouterModule } from '@angular/router';
import { LoginEffects } from './store/effects/login.effects';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { LoginPageComponent } from './containers/login-page/login-page.component';
import { ForgotPasswordPageComponent } from './containers/forgot-password-page/forgot-password-page.component';
import  { fromForgotPassword, fromLogin } from './store/reducers';
import { ForgotPasswordEffects } from './store/effects/forgot-password.effects';
import { environment } from '../../environments/environment';
import { ResetPasswordFormComponent } from './components/reset-password-form/reset-password-form.component';
import { ResetPasswordPageComponent } from './containers/reset-password-page/reset-password-page.component';
import { resetPasswordReducer } from './store/reducers/reset-password.reducer';
import { PasswordResetService } from './services/password-reset.service';
import { LayoutComponent } from './components/layout/layout.component';
import { ResetPasswordEffects } from './store/effects/reset-password.effects';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, environment.translationUrl, '.json');
}

@NgModule({
  declarations: [
    SigninComponent,
    ForgotPasswordComponent,
    LoginFormComponent,
    LoginPageComponent,
    ForgotPasswordPageComponent,
    ResetPasswordFormComponent,
    ResetPasswordPageComponent,
    LayoutComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TranslateModule.forChild({
      defaultLanguage: environment.defaultLanguage,
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    RouterModule.forChild(signinRoutes),
    StoreModule.forFeature('login', fromLogin.loginReducer),
    StoreModule.forFeature('forgotPassword', fromForgotPassword.forgotPasswordReducer),
    StoreModule.forFeature('resetPassword', resetPasswordReducer),
    EffectsModule.forFeature([LoginEffects,ForgotPasswordEffects, ResetPasswordEffects])
   
  ],
  providers:[
    PasswordResetService
  ]
})
export class SigninModule { }
