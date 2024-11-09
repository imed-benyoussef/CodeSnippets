import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { signupRoutes } from './signup.routes';
import { SignupComponent } from './signup.component';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { environment } from '../../environments/environment';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '@shared';
import { RouterModule } from '@angular/router';
import { Action, ActionReducer, MetaReducer, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { signupReducer } from './store/reducers/signup.reducer';
import { SignupEffects } from './store/effects/signup.effects';
import { NameComponent } from './components/name/name.component';
import { EmailComponent } from './components/email/email.component';
import { EmailVerificationComponent } from './components/email-verification/email-verification.component';
import { PasswordComponent } from './components/password/password.component';
import { PhoneComponent } from './components/phone/phone.component';
import { PhoneVerificationComponent } from './components/phone-verification/phone-verification.component';
import { ReviewComponent } from './components/review/review.component';
import { TermsComponent } from './components/terms/terms.component';
import { GeneralInfoComponent } from './components/general-info/general-info.component';
import { LayoutComponent } from './components/layout/layout.component';
import { FooterComponent } from './components/footer/footer.component';
import { AuthInterceptor } from './signup.interceptor';
import { SignupService } from './services/signup.service';

import {
  EmailPageComponent, EmailVerificationPageComponent, GeneralInfoPageComponent,
  InitializingAccountPageComponent, NamePageComponent, PasswordPageComponent, PhonePageComponent, 
  PhoneVerificationPageComponent, ReviewAndTermsPageComponent, WelcomePageComponent
} from './containers';
import { UnauthorizedComponent } from './containers/unauthorized/unauthorized.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, environment.translationUrl, '.json');
}

export const metaReducers: MetaReducer<any>[] = [localStorageSyncReducer];

export function localStorageSyncReducer(reducer: ActionReducer<any>): ActionReducer<any> {
  return function (state, action: Action) {
    // Obtenez l'état actuel du réducteur
    const nextState = reducer(state, action);

    // Stockez l'état complet ou une partie dans localStorage
    if (nextState && nextState.token?.access_token) {
      localStorage.setItem('signup', JSON.stringify({ token: nextState.token }));
    }

    return nextState;
  };
}


@NgModule({
  declarations: [
    LayoutComponent,
    FooterComponent,
    SignupComponent,
    NameComponent,
    GeneralInfoComponent,
    EmailComponent,
    EmailVerificationComponent,
    PasswordComponent,
    PhoneComponent,
    PhoneVerificationComponent,
    ReviewComponent,
    TermsComponent,
    NamePageComponent,
    GeneralInfoPageComponent,
    EmailPageComponent,
    EmailVerificationPageComponent,
    PasswordPageComponent,
    PhonePageComponent,
    PhoneVerificationPageComponent,
    WelcomePageComponent,
    ReviewAndTermsPageComponent,
    InitializingAccountPageComponent,
    UnauthorizedComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    // BrowserAnimationsModule,
    TranslateModule.forChild({
      defaultLanguage: 'fr',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    RouterModule.forChild(signupRoutes),
    StoreModule.forFeature('signup', signupReducer, {

      // initialState: (initialStateService: InitialStateService) => initialStateService.getInitialState(),
      // metaReducers: [localStorageSyncReducer]

    }),
    EffectsModule.forFeature([SignupEffects]),

  ],
  providers: [
    SignupService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class SignupModule { }


