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
    loadChildren: () => import('./signin/signin.module').then(e => e.SigninModule)
    ,
  },
  {
    path: 'signup',
    loadChildren: () => import('./signup/signup.module').then(e => e.SignupModule),
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
