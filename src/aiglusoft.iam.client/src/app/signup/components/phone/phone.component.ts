import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-phone',
  template: `
    <app-layout
      [title]="'signup.components.phone.title' | translate">
      <form class="form-custom mt-3" [formGroup]="phoneForm" (ngSubmit)="onSubmit()" novalidate>
           <div class="mb-3">
          <label class="form-label" for="phone">{{ 'signup.components.phone.label' | translate }}</label>
          <input type="tel" class="form-control" id="phone" formControlName="phone" [ngClass]="{
                      'is-invalid':
                      phoneForm.get('phone')?.invalid && (phoneForm.get('phone')?.dirty || phoneForm.get('phone')?.touched)
                    }" />
            
          <div class="invalid-feedback" *ngIf="phoneForm.get('phone')?.invalid && (phoneForm.get('phone')?.dirty || phoneForm.get('phone')?.touched)">
            <small *ngIf="phoneForm.get('phone')?.errors?.['required']">
              {{ 'signup.components.phone.validation.required' | translate }}
            </small>
            <small *ngIf="phoneForm.get('phone')?.errors?.['pattern']">
              {{ 'signup.components.phone.validation.pattern' | translate }}
            </small>
          </div>
          <div id="emailHelp" class="form-text">{{ 'signup.components.phone.help' | translate }}</div>
        </div>
        <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
          <button class="btn btn-link shadow-none" type="button" (click)="onSkip()">{{ 'signup.common.skip' | translate }}</button>
          <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="phoneForm.invalid">{{ 'signup.common.next' | translate }}</button>
        </div>
      </form>
    </app-layout>
  `
})
export class PhoneComponent implements OnInit {
  @Output() skip = new EventEmitter<void>();
  @Output() submit = new EventEmitter<{ phone: string }>();
  phoneForm!: FormGroup;

  @Input() externalError$!: Observable<Error | null>; // Not used

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.phoneForm = this.fb.group({
      phone: ['', [/*Validators.required,*/ Validators.pattern('^[0-9]{10,15}$')]]
    });
  }

  onSkip() {
    this.skip.emit();
  }

  onSubmit() {
    if (this.phoneForm.valid) {
      this.submit.emit(this.phoneForm.value);
    }
  }
}
