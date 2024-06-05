// shared.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthCarouselComponent } from './components/auth-carousel/auth-carousel.component';
import { TogglePasswordVisibilityDirective } from './directive/toggle-password-visibility/toggle-password-visibility.directive';
import { FormFieldComponent } from './form-field/form-field.component';

@NgModule({
  declarations: [
    // Shared components, directives, pipes
       
    FormFieldComponent,
    TogglePasswordVisibilityDirective,
    AuthCarouselComponent, 
  ],
  imports: [
    CommonModule
  ],
  exports: [
    // Shared components, directives, pipes
       
    FormFieldComponent,
    TogglePasswordVisibilityDirective,
    AuthCarouselComponent, 
  ]
})
export class SharedModule { }
