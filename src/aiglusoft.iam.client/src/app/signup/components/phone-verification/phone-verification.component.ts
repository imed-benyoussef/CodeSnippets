import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-phone-verification',
  template: `

    <app-layout 
      title="signup.components.phoneVerification.title" 
      instructions="signup.components.phoneVerification.instructions" 
      [params]="{phoneNumber: phoneNumber}">
      
      <form class="form-custom mt-3" [formGroup]="phoneVerificationForm" (ngSubmit)="onSubmit()" novalidate>
        <div class="d-flex justify-content-center mb-3" (paste)="onPaste($event)">
          <ng-container formArrayName="code">
            <div *ngFor="let digit of code.controls; let i = index" class="m-2">
              <input 
                type="text" 
                class="form-control text-center rounded-1 p-1"
                maxlength="1"
                [formControlName]="i"
                (input)="onInputChange($event, i)"
                (keydown)="onKeyDown($event, i)"
                [id]="'digit-' + i"
                [ngClass]="{
                  'border border-2 border-success': digit.valid && (digit.dirty || digit.touched)
                }" />
            </div>
          </ng-container>
        </div>
        <div class="mt-5 d-grid gap-2 d-md-flex justify-content-between mt-3">
          <button class="btn btn-link shadow-none" type="button" (click)="onBack()">{{ 'signup.common.back' | translate }}</button>
          <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="phoneVerificationForm.invalid">{{ 'signup.common.next' | translate }}</button>
        </div>
      </form>
    </app-layout>
  `,
  styles: [`
    .form-control {
      width: 40px;
      height: 40px;
      font-size: 1.5rem;
    }
  `]
})
export class PhoneVerificationComponent implements OnInit {


  @Input() phoneNumber: string | undefined;
  @Output() back = new EventEmitter();
  @Output() submit = new EventEmitter<{ code: string }>();
  phoneVerificationForm!: FormGroup;

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit() {
    this.phoneVerificationForm = this.fb.group({
      code: this.fb.array(Array(6).fill('').map(() => this.fb.control('', [Validators.required, Validators.pattern('^[0-9]$')]))),
    });
  }

  get code(): FormArray {
    return this.phoneVerificationForm.get('code') as FormArray;
  }

  onInputChange(event: Event, index: number) {
    const input = event.target as HTMLInputElement;
    const nextInput = document.getElementById(`digit-${index + 1}`);
    if (input.value && nextInput) {
      (nextInput as HTMLInputElement).focus();
    }
  }

  onKeyDown(event: KeyboardEvent, index: number) {
    const input = event.target as HTMLInputElement;
    const prevInput = document.getElementById(`digit-${index - 1}`);
    if (event.key === 'Backspace' && !input.value && prevInput) {
      (prevInput as HTMLInputElement).focus();
      (prevInput as HTMLInputElement).value = '';
      this.code.controls[index - 1].setValue('');
    }
  }

  onPaste(event: ClipboardEvent) {
    const clipboardData = event.clipboardData;
    const pastedText = clipboardData?.getData('text');
    if (pastedText && /^\d{6}$/.test(pastedText)) {
      const values = pastedText.split('');
      values.forEach((value, index) => {
        if (this.code.controls[index]) {
          this.code.controls[index].setValue(value);
          this.code.controls[index].markAsDirty();
          this.code.controls[index].markAsTouched();
        }
      });
      this.phoneVerificationForm.updateValueAndValidity();
    }
    event.preventDefault();
  }

  onBack() {
    this.back.emit();
  }

  onSubmit() {
    if (this.phoneVerificationForm.valid) {
      const code = this.code.value.join('');
      this.submit.emit({ code: code });
    }
  }
}
