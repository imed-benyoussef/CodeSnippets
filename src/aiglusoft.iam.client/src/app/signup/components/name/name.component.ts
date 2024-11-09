import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-name',
  template: `
<app-layout title="signup.components.name.title" instructions="signup.components.name.instructions">
  <form class="form-custom mt-3" [formGroup]="nameForm" (ngSubmit)="onSubmit()" novalidate>
    <div class="mb-3">
      <label class="form-label" for="firstName">{{ 'signup.components.name.firstName.label' | translate }}</label>
      <input class="form-control" id="firstName" formControlName="firstName" [ngClass]="{
        'is-invalid': nameForm.get('firstName')?.invalid && (nameForm.get('firstName')?.dirty || nameForm.get('firstName')?.touched)
      }" />
      <div class="invalid-feedback" *ngIf="nameForm.get('firstName')?.invalid && (nameForm.get('firstName')?.dirty || nameForm.get('firstName')?.touched)">
        <small *ngIf="nameForm.get('firstName')?.errors?.['required']">{{ 'signup.components.name.firstName.validation.required' | translate }}</small>
        <small *ngIf="nameForm.get('firstName')?.errors?.['minlength']">{{ 'signup.components.name.firstName.validation.minLength' | translate: { minLength: 2 } }}</small>
      </div>
    </div>
    <div class="mb-5">
      <label class="form-label" for="lastName">{{ 'signup.components.name.lastName.label' | translate }}</label>
      <input class="form-control" id="lastName" formControlName="lastName" [ngClass]="{
        'is-invalid': nameForm.get('lastName')?.invalid && (nameForm.get('lastName')?.dirty || nameForm.get('lastName')?.touched)
      }" />
      <div class="invalid-feedback" *ngIf="nameForm.get('lastName')?.invalid && (nameForm.get('lastName')?.dirty || nameForm.get('lastName')?.touched)">
        <small *ngIf="nameForm.get('lastName')?.errors?.['required']">{{ 'signup.components.name.lastName.validation.required' | translate }}</small>
        <small *ngIf="nameForm.get('lastName')?.errors?.['minlength']">{{ 'signup.components.name.lastName.validation.minLength' | translate: { minLength: 2 } }}</small>
      </div>
    </div>
    <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-3">
      <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="nameForm.invalid">{{ 'signup.common.next' | translate }}</button>
    </div>
  </form>
</app-layout>


  `
})
export class NameComponent implements OnInit {

  @Output() submit = new EventEmitter<{ firstName: string, lastName: string }>();
  nameForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.nameForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  onSubmit() {
    if (this.nameForm.valid) {
      this.submit.emit(this.nameForm.value);
    }
  }
}
