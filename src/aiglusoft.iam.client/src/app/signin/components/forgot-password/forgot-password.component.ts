import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  @Input() forgotPasswordForm!: FormGroup;
  @Input() successMessage: string | null = null;
  @Input() errorMessage: string | null = null;
  @Output() submitForm = new EventEmitter<void>();

  onSubmit(): void {
    this.submitForm.emit();
  }
}
