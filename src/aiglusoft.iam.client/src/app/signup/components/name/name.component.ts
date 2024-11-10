import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-name',
  template: `
    <app-layout title="signup.components.name.title" instructions="signup.components.name.instructions">
      <form class="space-y-6 mt-3" [formGroup]="nameForm" (ngSubmit)="onSubmit()" novalidate>
      <div class="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
      <div class="sm:col-span-3">
          <label class="block text-sm font-medium text-gray-900" for="firstName">{{ 'signup.components.name.firstName.label' | translate }}</label>
          <div class="mt-2">
            <input type="text"
              class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-indigo-600 sm:text-sm"
              id="firstName" formControlName="firstName"
              placeholder="{{ 'signup.components.name.firstName.placeholder' | translate }}" [ngClass]="{
                'ring-red-500': nameForm.controls['firstName'].invalid && (nameForm.controls['firstName'].dirty || nameForm.controls['firstName'].touched)
              }" />
            <div *ngIf="nameForm.controls['firstName'].invalid && (nameForm.controls['firstName'].dirty || nameForm.controls['firstName'].touched)"
              class="text-red-500 text-sm mt-1">
              <div *ngIf="nameForm.controls['firstName'].errors?.['required']">
                {{ 'signup.components.name.firstName.validation.required' | translate }}
              </div>
              <div *ngIf="nameForm.controls['firstName'].errors?.['minlength']">
                {{ 'signup.components.name.firstName.validation.minLength' | translate: { minLength: 2 } }}
              </div>
            </div>
          </div>
        </div>
        <div class="sm:col-span-3">
          <label class="block text-sm font-medium text-gray-900" for="lastName">{{ 'signup.components.name.lastName.label' | translate }}</label>
          <div class="mt-2">
            <input type="text"
              class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-indigo-600 sm:text-sm"
              id="lastName" formControlName="lastName"
              placeholder="{{ 'signup.components.name.lastName.placeholder' | translate }}" [ngClass]="{
                'ring-red-500': nameForm.controls['lastName'].invalid && (nameForm.controls['lastName'].dirty || nameForm.controls['lastName'].touched)
              }" />
            <div *ngIf="nameForm.controls['lastName'].invalid && (nameForm.controls['lastName'].dirty || nameForm.controls['lastName'].touched)"
              class="text-red-500 text-sm mt-1">
              <div *ngIf="nameForm.controls['lastName'].errors?.['required']">
                {{ 'signup.components.name.lastName.validation.required' | translate }}
              </div>
              <div *ngIf="nameForm.controls['lastName'].errors?.['minlength']">
                {{ 'signup.components.name.lastName.validation.minLength' | translate: { minLength: 2 } }}
              </div>
            </div>
          </div>
        </div>
      </div>
        <div class="text-center mt-3">
          <button
            class="w-full flex justify-center rounded-md bg-green-500 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-green-600 disabled:opacity-50"
            type="submit" [disabled]="nameForm.invalid">
            {{ 'signup.common.next' | translate }}
          </button>
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
