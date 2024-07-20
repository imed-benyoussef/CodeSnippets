import { NgModule } from '@angular/core';
import { FallbackModule } from '@aiglusoft/angular';

@NgModule({
  imports: [
    FallbackModule.withAppName('Sign Up')
  ]
})
export class SignupFallbackModule { }
