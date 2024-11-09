import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-email',
  template: `
    <app-layout title="signup.components.email.title" instructions="signup.components.email.instructions">
      <form class="form-custom mt-3" [formGroup]="emailForm" (ngSubmit)="onSubmit()" novalidate>
        <div *ngIf="errorMessage" class="alert alert-danger text-center mt-3">
          {{ errorMessage }}
        </div>
        <div class="mb-3">
          <label class="form-label" for="email">{{ 'signup.components.email.label' | translate }}</label>
          <input type="email" class="form-control" id="email" formControlName="email" [ngClass]="{
                      'is-invalid':
                      emailForm.get('email')?.invalid && (emailForm.get('email')?.dirty || emailForm.get('email')?.touched)
                    }" />
          <div id="emailHelp" class="form-text">{{ 'signup.components.email.help' | translate }}</div>
          <div class="invalid-feedback" *ngIf="emailForm.get('email')?.invalid && (emailForm.get('email')?.dirty || emailForm.get('email')?.touched)">
            <small *ngIf="emailForm.get('email')?.errors?.['required']">
              {{ 'signup.components.email.validation.required' | translate }}
            </small>
            <small *ngIf="emailForm.get('email')?.errors?.['email']">
              {{ 'signup.components.email.validation.email' | translate }}
            </small>
          </div>
        </div>
        <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
          <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="emailForm.invalid">{{ 'signup.common.next' | translate }}</button>
        </div>
      </form>
    </app-layout>

  `
})
export class EmailComponent implements OnInit {

  @Input() errorMessage: string  | undefined | null;
  @Input() value: string | null | undefined;
  @Output() submit = new EventEmitter<{ email: string }>();
  emailForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.emailForm = this.fb.group({
      email: [this.value, [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    if (this.emailForm.valid) {
      this.submit.emit(this.emailForm.value);
    }
  }
}
