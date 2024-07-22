import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Action, ActionReducer, MetaReducer, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { RouterModule, Routes } from '@angular/router';
import { loadRemoteModule } from '@angular-architects/module-federation';
import { environment } from '../environments/environment';

const routes: Routes = [
  {

    path: 'dashboard',
    loadChildren: async () => {
      return loadRemoteModule({
        remoteEntry: `${environment.accountRemoteEntry}/remoteEntry.js`,
        remoteName: 'account',
        exposedModule: './AccountModule',
      }).then(m => m.AccountModule).catch(err => {
        console.error(err);
        return import('./fallbacks/account-fallback.module').then(e => e.AccountFallbackModule)
      });
    },
  },
  {

    path: 'signin',
    loadChildren: async () => {
      return loadRemoteModule({
        remoteEntry: `${environment.signinRemoteEntry}/remoteEntry.js`,
        remoteName: 'signin',
        exposedModule: './SigninModule',
      }).then(m => m.SigninModule).catch(err => {
        console.error(err);
        return import('./fallbacks/signin-fallback.module').then(e => e.SigninFallbackModule)
      });
    },
  },
  {
    path: 'signup',
    loadChildren: async () => {
      return loadRemoteModule({
        remoteEntry: `${environment.signupRemoteEntry}/remoteEntry.js`,
        remoteName: 'signup',
        exposedModule: './SignupModule',
      }).then(m => m.SignupModule).catch(err => {
        console.error(err);
        return import('./fallbacks/signup-fallback.module').then(e => e.SignupFallbackModule)
      });
    },
  },
  // Redirect to signin by default
  { path: '', redirectTo: '/signin', pathMatch: 'full' }, 
  // Wildcard route for a 404 page (not found)
  //{ path: '**', redirectTo: '/signup' }
];

@NgModule({
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    TranslateModule.forRoot(),
    RouterModule.forRoot(routes),
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument()

  ],

  providers: [provideHttpClient()]
})
export class AppModule {
  constructor(private translate: TranslateService) {
    // this.translate.setDefaultLang('fr');
     this.translate.use('fr');
  }
}
