import { NgModule } from '@angular/core';
import { FallbackModule } from '@aiglusoft/angular';




@NgModule({
  declarations: [],
  imports: [
    FallbackModule.withAppName('Sign In')
  ]
})
export class SigninFallbackModule { }
