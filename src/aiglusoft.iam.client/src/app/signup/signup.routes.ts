import { Routes } from '@angular/router';
import { SignupComponent } from './signup.component';
import { EmailVerificationPageComponent } from './containers/email-verification-page/email-verification-page.component';
import { EmailPageComponent } from './containers/email-page/email-page.component';
import { GeneralInfoPageComponent } from './containers/general-info-page/general-info-page.component';
import { NamePageComponent } from './containers/name-page/name-page.component';
import { PasswordPageComponent } from './containers/password-page/password-page.component';
import { PhonePageComponent } from './containers/phone-page/phone-page.component';
import { PhoneVerificationPageComponent } from './containers/phone-verification-page/phone-verification-page.component';
import { WelcomePageComponent } from './containers/welcome-page/welcome-page.component';
import { ReviewAndTermsPageComponent } from './containers/review-and-terms-page/review-and-terms-page.component';
import { InitializingAccountPageComponent } from './containers/initializing-account-page/initializing-account-page.component';
import { nameSetGuard } from './guards/name-set.guard';
import { generalInfoSetGuard } from './guards/general-info-set.guard';
import { accessTokenSetGuard } from './guards/access-token-set.guard';
import { emailSetGuard } from './guards/email-set.guard';
import { UnauthorizedComponent } from './containers/unauthorized/unauthorized.component';

export const signupRoutes: Routes = [{
  path: '', component: SignupComponent, children: [
    { path: '', redirectTo: 'name', pathMatch: 'full' },
    { path: 'name', component: NamePageComponent },
    { path: 'general-info', component: GeneralInfoPageComponent, canActivate: [nameSetGuard] },
    { path: 'email', component: EmailPageComponent, canActivate: [generalInfoSetGuard] },
    { path: 'email-verification', component: EmailVerificationPageComponent, canDeactivate: [emailSetGuard, accessTokenSetGuard,] },
    { path: 'password', component: PasswordPageComponent, canActivate: [accessTokenSetGuard,] },
    { path: 'phone', component: PhonePageComponent, canActivate: [ accessTokenSetGuard] },
    { path: 'phone-verification', component: PhoneVerificationPageComponent, canActivate: [ accessTokenSetGuard] },
    { path: 'review-and-terms', component: ReviewAndTermsPageComponent, canActivate: [ accessTokenSetGuard] },
    { path: 'welcome', component: WelcomePageComponent }

  ]
},
{ path: 'initializing-account', component: InitializingAccountPageComponent },
{ path: 'unauthorized', component: UnauthorizedComponent },
];


