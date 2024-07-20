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




function loadRemoteStylesheet(href: string): Promise<void> {
  return new Promise((resolve, reject) => {
    try {
      const link = document.createElement('link');
      link.rel = 'stylesheet';
      link.href = href;
      link.onload = () => resolve();
      link.onerror = (err) => {
        console.error(`Failed to load stylesheet at ${href}`, err);
        reject(new Error(`Failed to load stylesheet at ${href}`));
      };
      document.head.appendChild(link);
    } catch (error) {
      console.error(`Error while appending stylesheet to document: ${error}`);
      reject(new Error(`Error while appending stylesheet to document: ${error}`));
    }
  });
}




const routes: Routes = [
  {

    path: 'dashboard',
    loadChildren: async () => {
      //await loadRemoteStylesheet('https://account.intranet.aiglusoft.net/styles.css'); // Charger les styles de signin
      return loadRemoteModule({
        remoteEntry: 'https://account.intranet.aiglusoft.net/remoteEntry.js',
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
      //await loadRemoteStylesheet('https://signin.intranet.aiglusoft.net/styles.css'); // Charger les styles de signin
      return loadRemoteModule({
        remoteEntry: 'https://signin.intranet.aiglusoft.net/remoteEntry.js',
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
      //await loadRemoteStylesheet('https://signup.intranet.aiglusoft.net/styles.css'); // Charger les styles de signup
      return loadRemoteModule({
        remoteEntry: 'https://signup.intranet.aiglusoft.net/remoteEntry.js',
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
