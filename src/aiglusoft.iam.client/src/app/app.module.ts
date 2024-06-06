import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignupComponent } from './features/auth/components/signup/signup.component';
import { SigninComponent } from './features/auth/components/signin/signin.component';
import { FormFieldComponent } from './shared/form-field/form-field.component';
import { TogglePasswordVisibilityDirective } from './shared/directive/toggle-password-visibility/toggle-password-visibility.directive';
import { AuthCarouselComponent } from './shared/components/auth-carousel/auth-carousel.component';
import { EmailVerificationComponent } from './features/auth/components/email-verification/email-verification.component';
import { ForgotPasswordComponent } from './features/auth/components/forgot-password/forgot-password.component';
import { LockscreenComponent } from './features/auth/components/lockscreen/lockscreen.component';
import { RecoverPasswordComponent } from './features/auth/components/recover-password/recover-password.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AuthModule } from './features/auth/auth.module';
import { SharedModule } from './shared/shared.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';


// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,


  ],
  imports: [
    BrowserModule, 
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    TranslateModule.forRoot({
      defaultLanguage: 'fr',
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      }
  })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
