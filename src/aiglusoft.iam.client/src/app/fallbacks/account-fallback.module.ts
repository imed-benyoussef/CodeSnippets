import { NgModule } from '@angular/core';
import { FallbackModule } from '@aiglusoft/angular';




@NgModule({
  imports: [
    FallbackModule.withAppName('Account')
  ]
})
export class AccountFallbackModule { }
