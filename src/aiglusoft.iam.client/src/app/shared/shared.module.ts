// shared.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthCarouselComponent } from './components/auth-carousel/auth-carousel.component';
import { TogglePasswordVisibilityDirective } from './directive/toggle-password-visibility/toggle-password-visibility.directive';
import { FormFieldComponent } from './form-field/form-field.component';
import { TranslateModule } from '@ngx-translate/core';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    // Shared components, directives, pipes
       
    FormFieldComponent,
    TogglePasswordVisibilityDirective,
    AuthCarouselComponent, 
  ],
  imports: [
    CommonModule,
    TranslateModule,

    ReactiveFormsModule,
  ],
  exports: [
    // Shared components, directives, pipes
       
    FormFieldComponent,
    TogglePasswordVisibilityDirective,
    AuthCarouselComponent, 
    TranslateModule,

    ReactiveFormsModule,
  ]
})
export class SharedModule { }
