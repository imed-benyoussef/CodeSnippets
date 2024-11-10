import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Action, ActionReducer, MetaReducer, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { RouterModule, Routes } from '@angular/router';
import { environment } from '../environments/environment';

const routes: Routes = [
  // ...existing code...
  {
    path: '',
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'mfa',
    loadChildren: () => import('./features/mfa/mfa.module').then(m => m.MfaModule)
  },
  {
    path: 'account',
    loadChildren: () => import('./features/account/account.module').then(m => m.AccountModule)
  }
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
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument(),
    StoreModule.forRoot({}, {}),
    !environment.production ? StoreDevtoolsModule.instrument() : [],

  ],

  providers: [provideHttpClient(withInterceptorsFromDi())]
})
export class AppModule {
  constructor(private translate: TranslateService) {
    // this.translate.setDefaultLang('fr');
    this.translate.use('fr');
  }
}
