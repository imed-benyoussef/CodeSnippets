// src/app/signin/components/reset-password-form/reset-password-form.component.ts
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MustMatch } from '@shared/validators';

@Component({
  selector: 'app-reset-password-form',
  template: `
      <form class="form-custom mt-3" [formGroup]="resetPasswordForm" (ngSubmit)="onSubmit()" novalidate>
        <div *ngIf="errorMessage" class="alert alert-danger text-center mt-3">
          {{ errorMessage }}
        </div>
        <div class="mb-3">
          <label class="form-label" for="password">{{ 'signin.components.resetPassword.password.label' | translate }}</label>
          <input type="password" class="form-control" id="password" formControlName="password" [ngClass]="{
                      'is-invalid': 
                      resetPasswordForm.get('password')?.invalid && (resetPasswordForm.get('password')?.dirty || resetPasswordForm.get('password')?.touched)
                    }" />
          <div class="invalid-feedback" *ngIf="resetPasswordForm.get('password')?.invalid && (resetPasswordForm.get('password')?.dirty || resetPasswordForm.get('password')?.touched)">
            <small *ngIf="resetPasswordForm.get('password')?.errors?.['required']">
              {{ 'signin.components.resetPassword.password.validation.required' | translate }}
            </small>
          </div>
        </div>
        <div class="mb-3">
          <label class="form-label" for="confirmPassword">{{ 'signin.components.resetPassword.confirmPassword.label' | translate }}</label>
          <input type="password" class="form-control" id="confirmPassword" formControlName="confirmPassword" [ngClass]="{
                      'is-invalid': 
                      resetPasswordForm.get('confirmPassword')?.invalid && (resetPasswordForm.get('confirmPassword')?.dirty || resetPasswordForm.get('confirmPassword')?.touched)
                    }" />
          <div class="invalid-feedback" *ngIf="resetPasswordForm.get('confirmPassword')?.invalid && (resetPasswordForm.get('confirmPassword')?.dirty || resetPasswordForm.get('confirmPassword')?.touched)">
            <small *ngIf="resetPasswordForm.get('confirmPassword')?.errors?.['required']">
              {{ 'signin.components.resetPassword.confirmPassword.validation.required' | translate }}
            </small>
            <small *ngIf="resetPasswordForm.get('confirmPassword')?.errors?.['mustMatch']">
              {{ 'signin.components.resetPassword.confirmPassword.validation.mustMatch' | translate }}
            </small>
          </div>
        </div>
        <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
          <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="resetPasswordForm.invalid">{{ 'signin.common.submit' | translate }}</button>
        </div>
      </form>
  `
})
export class ResetPasswordFormComponent implements OnInit {
  @Input() errorMessage!: string | null;
  @Output() submitForm = new EventEmitter<{ password: string, confirmPassword: string }>();
  
  resetPasswordForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) {}

  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: MustMatch('password', 'confirmPassword') });
  }

  onSubmit() {
    if (this.resetPasswordForm.valid) {
      const { password, confirmPassword } = this.resetPasswordForm.value;
      this.submitForm.emit({ password, confirmPassword });
    }
  }
}
