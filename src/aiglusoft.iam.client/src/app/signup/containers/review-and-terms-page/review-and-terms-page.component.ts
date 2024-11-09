import { Component, OnInit, ViewChild } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { selectSignupState, selectUser } from '../../store/selectors/signup.selectors';
import { completeSignup } from '../../store/actions/signup.actions';
import { TermsComponent } from '../../components/terms/terms.component';
import { User } from '../../signup.models';

@Component({
  selector: 'app-review-and-terms-page',
  template: `
    <app-layout
      [title]="'signup.components.reviewAndTerms.title' | translate"
      [instructions]="'signup.components.reviewAndTerms.instructions' | translate">
      
      <app-review
        [userData]="user$ | async">
      </app-review>

      <form [formGroup]="termsForm" (ngSubmit)="onSubmit()">
        <app-terms #termsComponent [formGroup]="termsForm" (termsAccepted)="onTermsAccepted()"></app-terms>
        
        <div class="mt-5 d-grid gap-2 d-md-flex justify-content-md-end mt-3">
        <button class="btn btn-success rounded-pill shadow-none" type="submit" [disabled]="!termsForm.get('termsAccepted')?.value">{{ 'signup.common.submit' | translate }}</button>
        </div>
      </form>
    </app-layout>
  `
})
export class ReviewAndTermsPageComponent {
  termsAccepted: boolean = false;
  user$!: Observable<User | undefined>;
  termsForm!: FormGroup;

  @ViewChild('termsComponent') termsComponent!: TermsComponent;

  constructor(private store: Store, private fb: FormBuilder) {
    this.user$ =  this.store.pipe(select(selectUser));
    this.termsForm = this.fb.group({
      termsAccepted: [false, Validators.requiredTrue]
    });
   }


  onSubmit() {
    if (!this.termsForm.get('termsAccepted')?.value) {
      this.termsComponent.openTermsModal();
    } else {
      this.completeSignup();
    }
  }

  onTermsAccepted() {
    this.termsForm.get('termsAccepted')?.setValue(true);
    this.completeSignup();
  }

  completeSignup() {
    if (this.termsForm.valid) {
      this.store.dispatch(completeSignup());
    }
  }
}
