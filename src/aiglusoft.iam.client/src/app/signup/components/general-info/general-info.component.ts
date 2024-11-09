import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-general-info',
  template: `

<div class="mb-3 pb-3 text-center">
              <h4 class="fw-normal">
                {{ "signup.components.generalInfo.title" | translate:{appName: ('signup.common.appName' | translate)} }}
              </h4>
              <p class="text-muted mb-0">
              {{ 'signup.components.generalInfo.instructions' | translate }}
              </p>
            </div>
            <br class="mb-5 mt-5 ">

   
    <form class="form-custom mt-3" [formGroup]="generalInfoForm" (ngSubmit)="onSubmit()" novalidate>
      <div class="mb-3">
        <label class="form-label" for="birthDate">{{ 'signup.components.generalInfo.birthDate.label' | translate }}</label>
        <input type="date" class="form-control" id="birthDate" formControlName="birthDate" [ngClass]="{
                    'is-invalid':
                    generalInfoForm.get('birthDate')?.invalid && (generalInfoForm.get('birthDate')?.dirty || generalInfoForm.get('birthDate')?.touched)
                  }" />
        <div class="invalid-feedback" *ngIf="generalInfoForm.get('birthDate')?.invalid && (generalInfoForm.get('birthDate')?.dirty || generalInfoForm.get('birthDate')?.touched)">
          <small *ngIf="generalInfoForm.get('birthDate')?.errors?.['required']">
            {{ 'signup.components.generalInfo.birthDate.validation.required' | translate }}
          </small>
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label" for="gender">{{ 'signup.components.generalInfo.gender.label' | translate }}</label>
        <select class="form-control" id="gender" formControlName="gender" [ngClass]="{
                    'is-invalid':
                    generalInfoForm.get('gender')?.invalid && (generalInfoForm.get('gender')?.dirty || generalInfoForm.get('gender')?.touched)
                  }">
          <option value="">{{ 'signup.components.generalInfo.gender.placeholder' | translate }}</option>
          <option value="male">{{ 'signup.components.generalInfo.gender.male' | translate }}</option>
          <option value="female">{{ 'signup.components.generalInfo.gender.female' | translate }}</option>
          <option value="other">{{ 'signup.components.generalInfo.gender.other' | translate }}</option>
        </select>
        <div class="invalid-feedback" *ngIf="generalInfoForm.get('gender')?.invalid && (generalInfoForm.get('gender')?.dirty || generalInfoForm.get('gender')?.touched)">
          <small *ngIf="generalInfoForm.get('gender')?.errors?.['required']">
            {{ 'signup.components.generalInfo.gender.validation.required' | translate }}
          </small>
        </div>
      </div>
      <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
        <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="generalInfoForm.invalid">{{ 'signup.common.next' | translate }}</button>
      </div>
    </form>
  `
})
export class GeneralInfoComponent implements OnInit {
  @Output() submit = new EventEmitter<{ birthDate: string, gender: string }>();
  generalInfoForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.generalInfoForm = this.fb.group({
      birthDate: ['', [Validators.required]],
      gender: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.generalInfoForm.valid) {
      this.submit.emit(this.generalInfoForm.value);
    }
  }
}
