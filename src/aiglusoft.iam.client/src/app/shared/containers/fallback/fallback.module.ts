import { InjectionToken, ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FallbackComponent } from './fallback.component';
import { Routes, RouterModule } from '@angular/router';

export const APP_NAME = new InjectionToken<string>('appName');


const routes: Routes = [
  { path: '', component: FallbackComponent }
];

@NgModule({
  declarations: [
    FallbackComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [FallbackComponent]
})
export class FallbackModule {
  static withAppName(appName: string): ModuleWithProviders<FallbackModule> {
    return {
      ngModule: FallbackModule,
      providers: [
        { provide: APP_NAME, useValue: appName }
      ]
    };
  }
}
