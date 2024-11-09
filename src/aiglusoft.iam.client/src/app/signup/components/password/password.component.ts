import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MustMatch } from '../../validators/must-match.validator';

@Component({
  selector: 'app-password',
  template: `
<app-layout title="signup.components.password.title" instructions="signup.components.password.instructions">
    <form class="form-custom mt-3" [formGroup]="passwordForm" (ngSubmit)="onSubmit()" novalidate>
      <div class="mb-3">
        <label class="form-label" for="password">{{ 'signup.components.password.label' | translate }}</label>
        <input type="password" class="form-control" id="password" formControlName="password" [ngClass]="{
                    'is-invalid':
                    passwordForm.get('password')?.invalid && (passwordForm.get('password')?.dirty || passwordForm.get('password')?.touched)
                  }" />
        <div class="invalid-feedback" *ngIf="passwordForm.get('password')?.invalid && (passwordForm.get('password')?.dirty || passwordForm.get('password')?.touched)">
          <small *ngIf="passwordForm.get('password')?.errors?.['required']">
            {{ 'signup.components.password.validation.required' | translate }}
          </small>
          <small *ngIf="passwordForm.get('password')?.errors?.['minlength']">
            {{ 'signup.components.password.validation.minLength' | translate: { minLength: 8 } }}
          </small>
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label" for="confirmPassword">{{ 'signup.components.password.confirmLabel' | translate }}</label>
        <input type="password" class="form-control" id="confirmPassword" formControlName="confirmPassword" [ngClass]="{
                    'is-invalid':
                    passwordForm.get('confirmPassword')?.invalid && (passwordForm.get('confirmPassword')?.dirty || passwordForm.get('confirmPassword')?.touched)
                  }" />
        <div class="invalid-feedback" *ngIf="passwordForm.get('confirmPassword')?.invalid && (passwordForm.get('confirmPassword')?.dirty || passwordForm.get('confirmPassword')?.touched)">
          <small *ngIf="passwordForm.get('confirmPassword')?.errors?.['required']">
            {{ 'signup.components.password.validation.required' | translate }}
          </small>
          <small *ngIf="passwordForm.get('confirmPassword')?.errors?.['minlength']">
            {{ 'signup.components.password.validation.minLength' | translate: { minLength: 8 } }}
          </small>
          <small *ngIf="passwordForm.errors?.['mustMatch']">
            {{ 'signup.components.password.validation.mustMatch' | translate }}
          </small>
        </div>
      </div>
      <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
        <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="passwordForm.invalid">{{ 'signup.common.next' | translate }}</button>
      </div>
    </form>
</app-layout>
  `
})
export class PasswordComponent implements OnInit {
  @Output() submit = new EventEmitter<{ password: string, confirmPassword: string }>();
  passwordForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.passwordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]]
    }, {
      validators: MustMatch('password', 'confirmPassword')
    });
  }

  onSubmit() {
    if (this.passwordForm.valid) {
      this.submit.emit(this.passwordForm.value);
    }
  }
}
